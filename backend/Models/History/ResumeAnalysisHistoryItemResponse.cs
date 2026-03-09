namespace ResumeAnalyser.Api.Models.History;

public sealed class ResumeAnalysisHistoryItemResponse
{
    public Guid Id { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public string TargetJobTitle { get; set; } = string.Empty;

    public int OverallScore { get; set; }

    public int AtsScore { get; set; }

    public DateTime CreatedAt { get; set; }
}
