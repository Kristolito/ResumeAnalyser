using Microsoft.AspNetCore.Identity;

namespace ResumeAnalyser.Api.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public ICollection<ResumeUploadRecord> ResumeUploads { get; set; } = [];

    public ICollection<ResumeAnalysisRecord> ResumeAnalyses { get; set; } = [];
}
