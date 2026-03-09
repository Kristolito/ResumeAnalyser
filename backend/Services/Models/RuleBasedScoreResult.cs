using ResumeAnalyser.Api.Models;

namespace ResumeAnalyser.Api.Services.Models;

public sealed class RuleBasedScoreResult
{
    public int OverallScore { get; init; }

    public int AtsScore { get; init; }

    public ScoreBreakdownResponse ScoreBreakdown { get; init; } = new();
}
