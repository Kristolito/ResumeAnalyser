using System.Text.Json;
using System.Text.RegularExpressions;
using ResumeAnalyser.Api.Data;
using ResumeAnalyser.Api.Domain.Entities;
using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Services.Interfaces;
using ResumeAnalyser.Api.Services.Models;
using ResumeAnalyser.Api.Services.RuleBased;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class ResumeAnalysisService(
    IPdfTextExtractor pdfTextExtractor,
    AppDbContext dbContext,
    ILogger<ResumeAnalysisService> logger,
    IHostEnvironment hostEnvironment) : IResumeAnalysisService
{
    private static readonly Regex WordRegex = new(@"\b[\w+#./-]+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex BulletLineRegex = new(@"^\s*([\-*•]|[0-9]+\.)\s+", RegexOptions.Compiled | RegexOptions.Multiline);

    public async Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        var extractedResumeText = await pdfTextExtractor.ExtractTextAsync(request.File!, cancellationToken);
        var normalizedResumeText = RuleBasedTextNormalizer.NormalizeForMatching(extractedResumeText);
        var resumeLines = RuleBasedTextNormalizer.SplitMeaningfulLines(extractedResumeText);

        if (string.IsNullOrWhiteSpace(normalizedResumeText))
        {
            var fallback = BuildFallbackResponse("Resume text extraction returned empty content.");
            await SaveRecordAsync(request, fallback, cancellationToken);
            return fallback;
        }

        var keywordTargets = KeywordExtractionHelper.BuildKeywordTargets(
            request.TargetJobTitle,
            request.TargetJobDescription,
            request.Notes);

        var matchedKeywords = KeywordExtractionHelper.FindMatchedKeywords(normalizedResumeText, keywordTargets);
        var missingKeywords = KeywordExtractionHelper.FindMissingKeywords(normalizedResumeText, keywordTargets);
        var (sectionCount, missingSections) = SectionDetectionHelper.DetectSections(resumeLines);
        var (skillHits, skillCategoryCoverage, matchedSkills) = TechnicalSkillDetectionHelper.Detect(normalizedResumeText);
        var (numericCount, percentCount, currencyCount, scaleCount, actionVerbCount, impactPhraseCount) =
            AchievementDetectionHelper.Analyze(normalizedResumeText);

        var signals = new RuleBasedAnalysisSignals
        {
            ResumeWordCount = WordRegex.Matches(extractedResumeText).Count,
            ResumeLineCount = resumeLines.Count,
            SectionMatches = sectionCount,
            MissingSections = missingSections,
            BulletLineCount = BulletLineRegex.Matches(extractedResumeText).Count,
            KeywordTargetCount = keywordTargets.Count,
            KeywordMatchedCount = matchedKeywords.Count,
            MatchedKeywords = matchedKeywords,
            MissingKeywords = missingKeywords,
            TechnicalKeywordHits = skillHits,
            TechnicalCategoryCoverage = skillCategoryCoverage,
            MatchedTechnicalSkills = matchedSkills,
            NumericSignalHits = numericCount,
            PercentSignalHits = percentCount,
            CurrencySignalHits = currencyCount,
            ScaleSignalHits = scaleCount,
            AchievementVerbHits = actionVerbCount,
            ImpactPhraseHits = impactPhraseCount
        };

        var overallScore = RuleBasedScoringHelper.CalculateOverallScore(signals);
        var atsScore = RuleBasedScoringHelper.CalculateAtsScore(signals);

        var response = new ResumeAnalysisResponse
        {
            OverallScore = overallScore,
            AtsScore = atsScore,
            CandidateSummary = BuildCandidateSummary(signals, overallScore, atsScore),
            Strengths = BuildStrengths(signals),
            Weaknesses = BuildWeaknesses(signals),
            MissingKeywords = signals.MissingKeywords,
            Recommendations = BuildRecommendations(signals),
            DebugExtractedTextPreview = hostEnvironment.IsDevelopment()
                ? (normalizedResumeText.Length > 320 ? normalizedResumeText[..320] + "..." : normalizedResumeText)
                : null
        };

        logger.LogInformation(
            "Analysis completed for {FileName}. Overall={Overall}, ATS={Ats}, MatchedKeywords={Matched}/{TotalKeywords}, Skills={SkillHits}, Sections={Sections}.",
            request.File?.FileName,
            overallScore,
            atsScore,
            signals.KeywordMatchedCount,
            signals.KeywordTargetCount,
            signals.TechnicalKeywordHits,
            signals.SectionMatches);

        await SaveRecordAsync(request, response, cancellationToken);
        return response;
    }

    private async Task SaveRecordAsync(
        ResumeAnalysisRequest request,
        ResumeAnalysisResponse response,
        CancellationToken cancellationToken)
    {
        var storedPayload = new StoredAnalysisPayload
        {
            Analysis = response,
            TargetJobDescription = request.TargetJobDescription.Trim(),
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim()
        };

        dbContext.ResumeAnalysisRecords.Add(new ResumeAnalysisRecord
        {
            FileName = request.File?.FileName ?? "uploaded-resume.pdf",
            TargetJobTitle = request.TargetJobTitle.Trim(),
            OverallScore = response.OverallScore,
            AtsScore = response.AtsScore,
            ResultJson = JsonSerializer.Serialize(storedPayload),
            CreatedAtUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static List<string> BuildStrengths(RuleBasedAnalysisSignals signals)
    {
        var strengths = new List<string>();

        if (signals.SectionMatches >= 4)
        {
            strengths.Add("Core resume sections are present, giving the document a clear structure.");
        }

        if (signals.KeywordTargetCount > 0 && signals.KeywordMatchedCount >= Math.Max(5, signals.KeywordTargetCount / 2))
        {
            strengths.Add("Role alignment is solid based on target keyword coverage.");
        }

        if (signals.TechnicalCategoryCoverage >= 3)
        {
            strengths.Add("Technical skills span multiple categories expected in modern engineering roles.");
        }

        if (signals.NumericSignalHits + signals.PercentSignalHits + signals.CurrencySignalHits >= 5)
        {
            strengths.Add("Quantified achievement evidence is present and supports impact claims.");
        }

        if (signals.BulletLineCount >= 7)
        {
            strengths.Add("Bullet usage supports fast readability for recruiter and ATS scanning.");
        }

        if (strengths.Count == 0)
        {
            strengths.Add("Resume provides a base structure to build stronger role alignment.");
        }

        return strengths.Take(5).ToList();
    }

    private static List<string> BuildWeaknesses(RuleBasedAnalysisSignals signals)
    {
        var weaknesses = new List<string>();

        if (signals.MissingSections.Contains("summary", StringComparer.OrdinalIgnoreCase))
        {
            weaknesses.Add("A professional summary/profile section is missing or unclear.");
        }

        if (signals.MissingSections.Contains("experience", StringComparer.OrdinalIgnoreCase))
        {
            weaknesses.Add("Experience section detection is weak, reducing credibility and context.");
        }

        if (signals.KeywordTargetCount > 0 && signals.KeywordMatchedCount < Math.Max(4, signals.KeywordTargetCount / 3))
        {
            weaknesses.Add("Keyword alignment to the target role is currently limited.");
        }

        if (signals.TechnicalCategoryCoverage < 2)
        {
            weaknesses.Add("Technical coverage appears narrow across languages/frameworks/cloud/tooling.");
        }

        if (signals.AchievementVerbHits + signals.ImpactPhraseHits < 4 || signals.NumericSignalHits < 2)
        {
            weaknesses.Add("Achievement statements are not consistently supported by measurable outcomes.");
        }

        if (signals.ResumeWordCount < 220)
        {
            weaknesses.Add("Resume content may be too short to convey depth of experience.");
        }
        else if (signals.ResumeWordCount > 1300)
        {
            weaknesses.Add("Resume length may be too long for fast screening.");
        }

        if (weaknesses.Count == 0)
        {
            weaknesses.Add("No major blockers detected, but targeting can still be improved.");
        }

        return weaknesses.Take(5).ToList();
    }

    private static List<string> BuildRecommendations(RuleBasedAnalysisSignals signals)
    {
        var recommendations = new List<string>();

        if (signals.MissingSections.Contains("summary", StringComparer.OrdinalIgnoreCase))
        {
            recommendations.Add("Add a concise professional summary aligned to your target role.");
        }

        if (signals.BulletLineCount < 5)
        {
            recommendations.Add("Rewrite experience entries into achievement-focused bullet points.");
        }

        if (signals.NumericSignalHits + signals.PercentSignalHits + signals.CurrencySignalHits < 4)
        {
            recommendations.Add("Add quantified metrics (percentages, scale, revenue/cost impact) to key achievements.");
        }

        if (signals.MissingKeywords.Count > 0)
        {
            var topMissing = string.Join(", ", signals.MissingKeywords.Take(3));
            recommendations.Add($"Integrate missing role keywords such as {topMissing} in relevant experience bullets.");
        }

        var missingCloudTerms = signals.MissingKeywords
            .Where(keyword => TechnicalSkillCatalog.Categories["cloud"].Contains(keyword, StringComparer.OrdinalIgnoreCase))
            .Take(2)
            .ToList();
        if (missingCloudTerms.Count > 0)
        {
            recommendations.Add($"Strengthen cloud/platform alignment by adding evidence for {string.Join(", ", missingCloudTerms)}.");
        }

        if (signals.MissingSections.Contains("skills", StringComparer.OrdinalIgnoreCase) || signals.TechnicalCategoryCoverage < 3)
        {
            recommendations.Add("Improve the skills section by grouping languages, frameworks, cloud, databases, and DevOps tools.");
        }

        if (recommendations.Count == 0)
        {
            recommendations.Add("Maintain current structure and tailor wording per target job to maximize match quality.");
        }

        return recommendations.Distinct(StringComparer.OrdinalIgnoreCase).Take(6).ToList();
    }

    private static string BuildCandidateSummary(RuleBasedAnalysisSignals signals, int overallScore, int atsScore)
    {
        var alignment = signals.KeywordTargetCount switch
        {
            0 => "Keyword alignment could not be fully evaluated.",
            _ when signals.KeywordMatchedCount >= Math.Max(6, signals.KeywordTargetCount / 2) =>
                "Role alignment looks strong based on keyword and skill overlap.",
            _ when signals.KeywordMatchedCount >= Math.Max(3, signals.KeywordTargetCount / 3) =>
                "Role alignment is moderate with several coverage gaps.",
            _ => "Role alignment is currently low and needs focused keyword tuning."
        };

        var structure = signals.SectionMatches >= 4
            ? "The resume structure is generally clear."
            : "The resume structure needs clearer section organization.";

        var impact = signals.NumericSignalHits + signals.ImpactPhraseHits >= 5
            ? "Impact evidence is present with measurable outcomes."
            : "Impact evidence should be strengthened with quantified achievements.";

        return $"{structure} {alignment} {impact} Overall score: {overallScore}, ATS score: {atsScore}.";
    }

    private ResumeAnalysisResponse BuildFallbackResponse(string reason)
    {
        return new ResumeAnalysisResponse
        {
            OverallScore = 35,
            AtsScore = 30,
            CandidateSummary = reason,
            Strengths = ["Upload and request context were received successfully."],
            Weaknesses = ["Extracted resume text quality is insufficient for reliable analysis."],
            MissingKeywords = [],
            Recommendations =
            [
                "Upload a text-based PDF exported directly from your editor.",
                "If your PDF is scanned, run OCR first before uploading."
            ],
            DebugExtractedTextPreview = hostEnvironment.IsDevelopment() ? string.Empty : null
        };
    }
}
