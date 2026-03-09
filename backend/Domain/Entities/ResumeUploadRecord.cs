namespace ResumeAnalyser.Api.Domain.Entities;

public sealed class ResumeUploadRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }

    public string OriginalFileName { get; set; } = string.Empty;

    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<ResumeAnalysisRecord> Analyses { get; set; } = [];
}
