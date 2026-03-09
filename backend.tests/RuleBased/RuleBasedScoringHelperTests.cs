using ResumeAnalyser.Api.Services.Models;
using ResumeAnalyser.Api.Services.RuleBased;

namespace ResumeAnalyser.Api.Tests.RuleBased;

public sealed class RuleBasedScoringHelperTests
{
    [Fact]
    public void Scores_IncreaseForStrongerSignals()
    {
        var weak = new RuleBasedAnalysisSignals
        {
            ResumeWordCount = 140,
            ResumeLineCount = 12,
            SectionMatches = 1,
            MissingSections = ["summary", "experience", "skills", "education", "projects", "certifications"],
            BulletLineCount = 1,
            KeywordTargetCount = 12,
            KeywordMatchedCount = 2,
            TechnicalKeywordHits = 2,
            TechnicalCategoryCoverage = 1,
            AchievementVerbHits = 1,
            NumericSignalHits = 0,
            PercentSignalHits = 0,
            CurrencySignalHits = 0,
            ScaleSignalHits = 0,
            ImpactPhraseHits = 0
        };

        var strong = new RuleBasedAnalysisSignals
        {
            ResumeWordCount = 650,
            ResumeLineCount = 55,
            SectionMatches = 6,
            MissingSections = [],
            BulletLineCount = 18,
            KeywordTargetCount = 12,
            KeywordMatchedCount = 9,
            TechnicalKeywordHits = 12,
            TechnicalCategoryCoverage = 5,
            AchievementVerbHits = 7,
            NumericSignalHits = 6,
            PercentSignalHits = 3,
            CurrencySignalHits = 1,
            ScaleSignalHits = 3,
            ImpactPhraseHits = 4
        };

        var weakScores = RuleBasedScoringHelper.Calculate(weak);
        var strongScores = RuleBasedScoringHelper.Calculate(strong);

        Assert.True(strongScores.OverallScore > weakScores.OverallScore);
        Assert.True(strongScores.AtsScore > weakScores.AtsScore);
        Assert.True(strongScores.ScoreBreakdown.Structure >= weakScores.ScoreBreakdown.Structure);
    }
}
