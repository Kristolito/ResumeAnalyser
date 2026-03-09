namespace ResumeAnalyser.Api.Models.History;

public sealed class ResumeAnalysisHistoryDetailResponse
{
    public Guid Id { get; set; }

    public int OverallScore { get; set; }

    public int AtsScore { get; set; }

    public string CandidateSummary { get; set; } = string.Empty;

    public List<string> Strengths { get; set; } = [];

    public List<string> Weaknesses { get; set; } = [];

    public List<string> MissingKeywords { get; set; } = [];

    public List<string> Recommendations { get; set; } = [];

    public string TargetJobTitle { get; set; } = string.Empty;

    public string TargetJobDescription { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
