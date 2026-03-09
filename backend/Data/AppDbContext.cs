using Microsoft.EntityFrameworkCore;
using ResumeAnalyser.Api.Domain.Entities;

namespace ResumeAnalyser.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ResumeAnalysisRecord> ResumeAnalysisRecords => Set<ResumeAnalysisRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResumeAnalysisRecord>(entity =>
        {
            entity.ToTable("resume_analysis_records");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.TargetJobTitle).HasMaxLength(150).IsRequired();
            entity.Property(x => x.ResultJson).HasColumnType("longtext").IsRequired();
            entity.Property(x => x.CreatedAtUtc).IsRequired();
        });
    }
}
