namespace ResumeAnalyser.Api.Services.Models;

public sealed class RuleBasedAnalysisSignals
{
    public int ResumeWordCount { get; init; }

    public int SectionMatches { get; init; }

    public int BulletLineCount { get; init; }

    public int KeywordTargetCount { get; init; }

    public int KeywordMatchedCount { get; init; }

    public int TechnicalKeywordHits { get; init; }

    public int AchievementVerbHits { get; init; }

    public int NumericSignalHits { get; init; }

    public List<string> MissingKeywords { get; init; } = [];
}
