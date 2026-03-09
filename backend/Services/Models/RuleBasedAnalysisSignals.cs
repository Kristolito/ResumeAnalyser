namespace ResumeAnalyser.Api.Services.Models;

public sealed class RuleBasedAnalysisSignals
{
    public int ResumeWordCount { get; init; }

    public int ResumeLineCount { get; init; }

    public int SectionMatches { get; init; }

    public List<string> MissingSections { get; init; } = [];

    public int BulletLineCount { get; init; }

    public int KeywordTargetCount { get; init; }

    public int KeywordMatchedCount { get; init; }

    public List<string> MatchedKeywords { get; init; } = [];

    public int TechnicalKeywordHits { get; init; }

    public int TechnicalCategoryCoverage { get; init; }

    public List<string> MatchedTechnicalSkills { get; init; } = [];

    public int AchievementVerbHits { get; init; }

    public int NumericSignalHits { get; init; }

    public int PercentSignalHits { get; init; }

    public int CurrencySignalHits { get; init; }

    public int ScaleSignalHits { get; init; }

    public int ImpactPhraseHits { get; init; }

    public List<string> MissingKeywords { get; init; } = [];
}
