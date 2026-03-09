using System.Text.RegularExpressions;
using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Services.Interfaces;
using ResumeAnalyser.Api.Services.Models;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class ResumeAnalysisService(
    IPdfTextExtractor pdfTextExtractor,
    ILogger<ResumeAnalysisService> logger,
    IHostEnvironment hostEnvironment) : IResumeAnalysisService
{
    private static readonly Regex WordRegex = new(@"\b[\w+#.]+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex BulletLineRegex = new(@"^\s*([\-*•]|[0-9]+\.)\s+", RegexOptions.Compiled | RegexOptions.Multiline);
    private static readonly Regex NumericSignalRegex = new(@"\b\d+([.,]\d+)?(%|k|m|b)?\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex MultiSpaceRegex = new(@"\s+", RegexOptions.Compiled);

    private static readonly string[] SectionHeadings =
    [
        "summary", "profile", "experience", "work experience", "employment", "skills", "technical skills", "education",
        "projects", "certifications"
    ];

    private static readonly string[] AchievementVerbs =
    [
        "increased", "reduced", "improved", "delivered", "led", "built", "optimized", "launched", "implemented",
        "designed", "automated", "scaled", "mentored", "owned", "achieved"
    ];

    private static readonly string[] TechnicalTerms =
    [
        "c#", ".net", "java", "python", "javascript", "typescript", "go", "sql", "react", "angular", "vue", "node",
        "asp.net", "spring", "django", "flask", "kubernetes", "docker", "aws", "azure", "gcp", "mysql", "postgresql",
        "mongodb", "redis", "ci/cd", "terraform", "git", "microservices", "rest api"
    ];

    private static readonly HashSet<string> StopWords = new(
    [
        "the", "and", "for", "with", "you", "your", "our", "from", "that", "this", "will", "are", "was", "were", "have",
        "has", "had", "into", "over", "under", "using", "use", "used", "ability", "skills", "skill", "years", "year",
        "role", "job", "work", "team", "candidate", "strong", "experience", "including", "required", "preferred", "plus",
        "their", "they", "them", "who", "all", "any", "not", "but", "can", "should", "must", "high", "good", "well",
        "across", "through", "within", "about", "such", "also", "more", "than", "etc"
    ], StringComparer.OrdinalIgnoreCase);

    public async Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        var resumeText = await pdfTextExtractor.ExtractTextAsync(request.File!, cancellationToken);
        var normalizedResumeText = NormalizeText(resumeText);

        if (string.IsNullOrWhiteSpace(normalizedResumeText))
        {
            return BuildFallbackResponse("Resume text could not be extracted clearly.", hostEnvironment.IsDevelopment(), string.Empty);
        }

        var keywordTargets = BuildKeywordTargets(
            request.TargetJobTitle,
            request.TargetJobDescription,
            request.Notes);

        var signals = CollectSignals(normalizedResumeText, keywordTargets);
        var overallScore = CalculateOverallScore(signals);
        var atsScore = CalculateAtsScore(signals);

        var strengths = BuildStrengths(signals, overallScore, atsScore);
        var weaknesses = BuildWeaknesses(signals, overallScore, atsScore);
        var recommendations = BuildRecommendations(signals, weaknesses, keywordTargets.Count);

        var summary = BuildCandidateSummary(signals, overallScore, atsScore);
        var previewText = normalizedResumeText.Length > 320 ? normalizedResumeText[..320] + "..." : normalizedResumeText;

        logger.LogInformation(
            "Rule-based analysis completed for {FileName}. Overall: {OverallScore}, ATS: {AtsScore}, Words: {WordCount}, Keywords: {KeywordMatched}/{KeywordTarget}.",
            request.File?.FileName,
            overallScore,
            atsScore,
            signals.ResumeWordCount,
            signals.KeywordMatchedCount,
            signals.KeywordTargetCount);

        return new ResumeAnalysisResponse
        {
            OverallScore = overallScore,
            AtsScore = atsScore,
            CandidateSummary = summary,
            Strengths = strengths,
            Weaknesses = weaknesses,
            MissingKeywords = signals.MissingKeywords,
            Recommendations = recommendations,
            DebugExtractedTextPreview = hostEnvironment.IsDevelopment() ? previewText : null
        };
    }

    private static RuleBasedAnalysisSignals CollectSignals(string resumeText, List<string> keywordTargets)
    {
        var lowerResumeText = resumeText.ToLowerInvariant();
        var words = WordRegex.Matches(resumeText);
        var bulletMatches = BulletLineRegex.Matches(resumeText);
        var numericSignals = NumericSignalRegex.Matches(resumeText).Count;

        var sectionMatches = SectionHeadings.Count(section => ContainsPhrase(lowerResumeText, section));
        var keywordMatchedCount = keywordTargets.Count(keyword => ContainsPhrase(lowerResumeText, keyword));
        var missingKeywords = keywordTargets
            .Where(keyword => !ContainsPhrase(lowerResumeText, keyword))
            .Take(8)
            .ToList();

        var technicalHits = TechnicalTerms.Count(term => ContainsPhrase(lowerResumeText, term));
        var achievementVerbHits = AchievementVerbs.Count(verb => ContainsPhrase(lowerResumeText, verb));

        return new RuleBasedAnalysisSignals
        {
            ResumeWordCount = words.Count,
            SectionMatches = sectionMatches,
            BulletLineCount = bulletMatches.Count,
            KeywordTargetCount = keywordTargets.Count,
            KeywordMatchedCount = keywordMatchedCount,
            TechnicalKeywordHits = technicalHits,
            AchievementVerbHits = achievementVerbHits,
            NumericSignalHits = numericSignals,
            MissingKeywords = missingKeywords
        };
    }

    private static int CalculateOverallScore(RuleBasedAnalysisSignals signals)
    {
        var lengthScore = ScoreLength(signals.ResumeWordCount);               // 0-20
        var sectionScore = ScaleScore(signals.SectionMatches, 6, 20);         // 0-20
        var keywordScore = ScoreKeywordAlignment(signals, 25);                // 0-25
        var technicalScore = ScaleScore(signals.TechnicalKeywordHits, 12, 15); // 0-15
        var achievementScore = ScoreAchievementSignals(signals);              // 0-20

        return ClampScore(lengthScore + sectionScore + keywordScore + technicalScore + achievementScore);
    }

    private static int CalculateAtsScore(RuleBasedAnalysisSignals signals)
    {
        var sectionScore = ScaleScore(signals.SectionMatches, 6, 35);         // 0-35
        var bulletScore = ScaleScore(signals.BulletLineCount, 10, 20);        // 0-20
        var keywordScore = ScoreKeywordAlignment(signals, 35);                // 0-35
        var lengthScore = ScaleScore(ScoreLength(signals.ResumeWordCount), 20, 10); // 0-10

        return ClampScore(sectionScore + bulletScore + keywordScore + lengthScore);
    }

    private static int ScoreLength(int wordCount)
    {
        return wordCount switch
        {
            < 180 => 5,
            <= 300 => 14,
            <= 900 => 20,
            <= 1300 => 12,
            _ => 7
        };
    }

    private static int ScoreKeywordAlignment(RuleBasedAnalysisSignals signals, int maxScore)
    {
        if (signals.KeywordTargetCount == 0)
        {
            return (int)Math.Round(maxScore * 0.55);
        }

        return ScaleScore(signals.KeywordMatchedCount, signals.KeywordTargetCount, maxScore);
    }

    private static int ScoreAchievementSignals(RuleBasedAnalysisSignals signals)
    {
        var verbScore = ScaleScore(signals.AchievementVerbHits, 8, 10);
        var numericScore = ScaleScore(signals.NumericSignalHits, 8, 10);
        return ClampScore(verbScore + numericScore, 20);
    }

    private static int ScaleScore(int value, int expectedMax, int scoreMax)
    {
        if (expectedMax <= 0 || scoreMax <= 0)
        {
            return 0;
        }

        var ratio = Math.Min(1d, Math.Max(0d, value / (double)expectedMax));
        return (int)Math.Round(ratio * scoreMax);
    }

    private static int ClampScore(int score, int max = 100) => Math.Min(max, Math.Max(0, score));

    private static string NormalizeText(string text)
    {
        return MultiSpaceRegex.Replace(text, " ").Trim();
    }

    private static List<string> BuildKeywordTargets(string jobTitle, string jobDescription, string? notes)
    {
        var source = $"{jobTitle} {jobDescription} {notes}";
        var tokens = WordRegex.Matches(source.ToLowerInvariant())
            .Select(match => match.Value.Trim())
            .Where(token => token.Length >= 4)
            .Where(token => !StopWords.Contains(token))
            .ToList();

        var ranked = tokens
            .GroupBy(token => token)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group => group.Key)
            .Take(20)
            .ToList();

        return ranked;
    }

    private static List<string> BuildStrengths(RuleBasedAnalysisSignals signals, int overallScore, int atsScore)
    {
        var strengths = new List<string>();

        if (signals.ResumeWordCount >= 250 && signals.ResumeWordCount <= 1000)
        {
            strengths.Add("Resume length appears appropriately detailed for screening.");
        }

        if (signals.SectionMatches >= 4)
        {
            strengths.Add("Common resume sections are present, improving readability.");
        }

        if (signals.BulletLineCount >= 6)
        {
            strengths.Add("Bullet points are used consistently to structure experience.");
        }

        if (signals.KeywordTargetCount > 0 && signals.KeywordMatchedCount >= Math.Max(4, signals.KeywordTargetCount / 2))
        {
            strengths.Add("Resume aligns with a meaningful share of target role keywords.");
        }

        if (signals.TechnicalKeywordHits >= 6)
        {
            strengths.Add("Technical stack signals are clearly visible.");
        }

        if (signals.NumericSignalHits >= 4 || signals.AchievementVerbHits >= 4)
        {
            strengths.Add("Achievement-oriented language and measurable outcomes are present.");
        }

        if (overallScore >= 70 && atsScore >= 65)
        {
            strengths.Add("Overall structure and targeting are in a good baseline range.");
        }

        if (strengths.Count == 0)
        {
            strengths.Add("Resume includes core content to begin targeted improvements.");
        }

        return strengths.Take(5).ToList();
    }

    private static List<string> BuildWeaknesses(RuleBasedAnalysisSignals signals, int overallScore, int atsScore)
    {
        var weaknesses = new List<string>();

        if (signals.ResumeWordCount < 200)
        {
            weaknesses.Add("Resume may be too short to fully communicate relevant experience.");
        }
        else if (signals.ResumeWordCount > 1200)
        {
            weaknesses.Add("Resume may be too long and could lose focus for quick screening.");
        }

        if (signals.SectionMatches < 3)
        {
            weaknesses.Add("Several expected sections are missing or unclear.");
        }

        if (signals.BulletLineCount < 3)
        {
            weaknesses.Add("Limited bullet structure can reduce readability.");
        }

        if (signals.KeywordTargetCount > 0 && signals.KeywordMatchedCount < Math.Max(3, signals.KeywordTargetCount / 3))
        {
            weaknesses.Add("Keyword alignment with the target job description is currently low.");
        }

        if (signals.TechnicalKeywordHits < 4)
        {
            weaknesses.Add("Technical signals could be clearer for role matching.");
        }

        if (signals.NumericSignalHits < 2 && signals.AchievementVerbHits < 3)
        {
            weaknesses.Add("Achievement impact is underrepresented with measurable outcomes.");
        }

        if (overallScore < 55 || atsScore < 55)
        {
            weaknesses.Add("Current structure and targeting may limit interview conversion.");
        }

        if (weaknesses.Count == 0)
        {
            weaknesses.Add("No major structural issues detected, but optimization opportunities remain.");
        }

        return weaknesses.Take(5).ToList();
    }

    private static List<string> BuildRecommendations(
        RuleBasedAnalysisSignals signals,
        IReadOnlyCollection<string> weaknesses,
        int keywordPoolSize)
    {
        var recommendations = new List<string>();

        if (signals.SectionMatches < 4)
        {
            recommendations.Add("Add clear section headers for summary, experience, skills, and education.");
        }

        if (signals.BulletLineCount < 5)
        {
            recommendations.Add("Rewrite experience entries into concise achievement-focused bullet points.");
        }

        if (signals.NumericSignalHits < 3)
        {
            recommendations.Add("Include measurable outcomes such as percentages, scale, or impact metrics.");
        }

        if (signals.MissingKeywords.Count > 0)
        {
            recommendations.Add("Integrate missing target-role keywords naturally into relevant experience lines.");
        }

        if (signals.TechnicalKeywordHits < 5)
        {
            recommendations.Add("Clarify technical skills by grouping languages, frameworks, cloud, and tools.");
        }

        if (signals.ResumeWordCount < 200)
        {
            recommendations.Add("Expand recent role details with responsibilities and quantified outcomes.");
        }
        else if (signals.ResumeWordCount > 1200)
        {
            recommendations.Add("Trim lower-impact details to keep the resume focused and scannable.");
        }

        if (weaknesses.Any(weakness => weakness.Contains("keyword alignment", StringComparison.OrdinalIgnoreCase)) &&
            keywordPoolSize > 0)
        {
            recommendations.Add("Tailor the summary and top experience bullets to mirror the target role language.");
        }

        if (recommendations.Count == 0)
        {
            recommendations.Add("Maintain current structure and continue tailoring the resume for each target role.");
        }

        return recommendations.Distinct(StringComparer.OrdinalIgnoreCase).Take(6).ToList();
    }

    private static string BuildCandidateSummary(RuleBasedAnalysisSignals signals, int overallScore, int atsScore)
    {
        var structureSummary = signals.SectionMatches >= 4 && signals.BulletLineCount >= 5
            ? "The resume appears reasonably well structured."
            : "The resume structure needs clearer sectioning and formatting signals.";

        var alignmentSummary = signals.KeywordTargetCount == 0
            ? "Target role keyword alignment could not be evaluated in depth."
            : signals.KeywordMatchedCount >= Math.Max(4, signals.KeywordTargetCount / 2)
                ? "It shows useful alignment with the target role keywords."
                : "Alignment with the target role keywords is currently moderate to low.";

        var impactSummary = signals.NumericSignalHits >= 3 || signals.AchievementVerbHits >= 4
            ? "Achievement and impact signals are present."
            : "Achievement evidence should be strengthened with more measurable outcomes.";

        return $"{structureSummary} {alignmentSummary} {impactSummary} " +
               $"Overall score: {overallScore}, screening compatibility score: {atsScore}.";
    }

    private static bool ContainsPhrase(string text, string phrase)
    {
        var escaped = Regex.Escape(phrase);
        return Regex.IsMatch(text, $@"\b{escaped}\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private static ResumeAnalysisResponse BuildFallbackResponse(
        string summary,
        bool includeDebugPreview,
        string extractedPreview)
    {
        return new ResumeAnalysisResponse
        {
            OverallScore = 35,
            AtsScore = 30,
            CandidateSummary = summary,
            Strengths = ["Upload and role details were received successfully."],
            Weaknesses = ["Resume text quality is too low for reliable analysis."],
            MissingKeywords = [],
            Recommendations =
            [
                "Upload a text-based PDF exported directly from your editor.",
                "Ensure the PDF content is selectable text, not only scanned images."
            ],
            DebugExtractedTextPreview = includeDebugPreview ? extractedPreview : null
        };
    }
}
