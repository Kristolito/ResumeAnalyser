using ResumeAnalyser.Api.Services.Models;

namespace ResumeAnalyser.Api.Services.RuleBased;

public static class RuleBasedScoringHelper
{
    public static int CalculateOverallScore(RuleBasedAnalysisSignals signals)
    {
        var structureScore = WeightedStructureScore(signals);         // 0-25
        var keywordScore = WeightedKeywordScore(signals, 30);         // 0-30
        var skillsScore = WeightedSkillsScore(signals);               // 0-20
        var achievementScore = WeightedAchievementScore(signals);     // 0-15
        var readabilityScore = WeightedReadabilityScore(signals);     // 0-10

        return Clamp(structureScore + keywordScore + skillsScore + achievementScore + readabilityScore);
    }

    public static int CalculateAtsScore(RuleBasedAnalysisSignals signals)
    {
        var structureScore = Scale(signals.SectionMatches, 6, 35);              // 0-35
        var keywordScore = WeightedKeywordScore(signals, 40);                    // 0-40
        var readabilityScore = Scale(signals.BulletLineCount, 12, 15);           // 0-15
        var skillsScore = Scale(signals.TechnicalCategoryCoverage, 4, 10);       // 0-10

        return Clamp(structureScore + keywordScore + readabilityScore + skillsScore);
    }

    private static int WeightedStructureScore(RuleBasedAnalysisSignals signals)
    {
        var sectionScore = Scale(signals.SectionMatches, 6, 18);
        var summaryBonus = signals.MissingSections.Contains("summary", StringComparer.OrdinalIgnoreCase) ? 0 : 3;
        var experienceBonus = signals.MissingSections.Contains("experience", StringComparer.OrdinalIgnoreCase) ? 0 : 2;
        var skillsBonus = signals.MissingSections.Contains("skills", StringComparer.OrdinalIgnoreCase) ? 0 : 2;

        return Clamp(sectionScore + summaryBonus + experienceBonus + skillsBonus, 25);
    }

    private static int WeightedKeywordScore(RuleBasedAnalysisSignals signals, int maxScore)
    {
        if (signals.KeywordTargetCount <= 0)
        {
            return (int)Math.Round(maxScore * 0.5);
        }

        return Scale(signals.KeywordMatchedCount, signals.KeywordTargetCount, maxScore);
    }

    private static int WeightedSkillsScore(RuleBasedAnalysisSignals signals)
    {
        var categoryScore = Scale(signals.TechnicalCategoryCoverage, 6, 10);
        var depthScore = Scale(signals.TechnicalKeywordHits, 16, 10);
        return Clamp(categoryScore + depthScore, 20);
    }

    private static int WeightedAchievementScore(RuleBasedAnalysisSignals signals)
    {
        var quantifiedScore = Scale(
            signals.NumericSignalHits + signals.PercentSignalHits + signals.CurrencySignalHits,
            12,
            8);
        var impactScore = Scale(
            signals.AchievementVerbHits + signals.ImpactPhraseHits + signals.ScaleSignalHits,
            12,
            7);

        return Clamp(quantifiedScore + impactScore, 15);
    }

    private static int WeightedReadabilityScore(RuleBasedAnalysisSignals signals)
    {
        var bulletScore = Scale(signals.BulletLineCount, 10, 5);

        var wordCountScore = signals.ResumeWordCount switch
        {
            < 180 => 1,
            <= 320 => 4,
            <= 900 => 5,
            <= 1300 => 3,
            _ => 2
        };

        return Clamp(bulletScore + wordCountScore, 10);
    }

    private static int Scale(int value, int expectedMax, int scoreMax)
    {
        if (expectedMax <= 0 || scoreMax <= 0)
        {
            return 0;
        }

        var ratio = Math.Clamp(value / (double)expectedMax, 0d, 1d);
        return (int)Math.Round(ratio * scoreMax);
    }

    private static int Clamp(int value, int max = 100) => Math.Clamp(value, 0, max);
}
