using ResumeAnalyser.Api.Models;

namespace ResumeAnalyser.Api.Services.Models;

public sealed class StoredAnalysisPayload
{
    public ResumeAnalysisResponse Analysis { get; set; } = new();

    public string TargetJobDescription { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
