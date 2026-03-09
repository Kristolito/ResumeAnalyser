using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResumeAnalyser.Api.Domain.Entities;

namespace ResumeAnalyser.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ResumeAnalysisRecord> ResumeAnalysisRecords => Set<ResumeAnalysisRecord>();
    public DbSet<ResumeUploadRecord> ResumeUploadRecords => Set<ResumeUploadRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ResumeUploadRecord>(entity =>
        {
            entity.ToTable("resume_upload_records");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OriginalFileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.UploadedAtUtc).IsRequired();

            entity
                .HasOne(x => x.User)
                .WithMany(x => x.ResumeUploads)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ResumeAnalysisRecord>(entity =>
        {
            entity.ToTable("resume_analysis_records");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.ResumeUploadRecordId).IsRequired();
            entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.TargetJobTitle).HasMaxLength(150).IsRequired();
            entity.Property(x => x.ResultJson).HasColumnType("longtext").IsRequired();
            entity.Property(x => x.CreatedAtUtc).IsRequired();

            entity
                .HasOne(x => x.User)
                .WithMany(x => x.ResumeAnalyses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(x => x.ResumeUpload)
                .WithMany(x => x.Analyses)
                .HasForeignKey(x => x.ResumeUploadRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
