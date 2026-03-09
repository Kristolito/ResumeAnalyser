namespace ResumeAnalyser.Api.Domain.Entities;

public sealed class ResumeAnalysisRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }

    public Guid ResumeUploadRecordId { get; set; }

    public ResumeUploadRecord? ResumeUpload { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string TargetJobTitle { get; set; } = string.Empty;

    public int OverallScore { get; set; }

    public int AtsScore { get; set; }

    public string ResultJson { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
