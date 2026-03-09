namespace ResumeAnalyser.Api.Services.Models;

public sealed class ResumeAnalysisInput
{
    public string FileName { get; init; } = string.Empty;

    public string ResumeText { get; init; } = string.Empty;

    public string TargetJobTitle { get; init; } = string.Empty;

    public string TargetJobDescription { get; init; } = string.Empty;

    public string? Notes { get; init; }
}
