namespace ResumeAnalyser.Api.Models;

public sealed class ResumeAnalysisResponse
{
    public int OverallScore { get; set; }

    public int AtsScore { get; set; }

    public string CandidateSummary { get; set; } = string.Empty;

    public List<string> Strengths { get; set; } = [];

    public List<string> Weaknesses { get; set; } = [];

    public List<string> MissingKeywords { get; set; } = [];

    public List<string> Recommendations { get; set; } = [];

    public ScoreBreakdownResponse? ScoreBreakdown { get; set; }

    // Development-only helper to verify PDF extraction output.
    public string? DebugExtractedTextPreview { get; set; }
}
