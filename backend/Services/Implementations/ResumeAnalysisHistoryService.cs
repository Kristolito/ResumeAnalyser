using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ResumeAnalyser.Api.Data;
using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Models.History;
using ResumeAnalyser.Api.Services.Interfaces;
using ResumeAnalyser.Api.Services.Models;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class ResumeAnalysisHistoryService(AppDbContext dbContext) : IResumeAnalysisHistoryService
{
    public async Task<IReadOnlyList<ResumeAnalysisHistoryItemResponse>> GetAnalysesAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeAnalysisRecords
            .AsNoTracking()
            .OrderByDescending(record => record.CreatedAtUtc)
            .Select(record => new ResumeAnalysisHistoryItemResponse
            {
                Id = record.Id,
                OriginalFileName = record.FileName,
                TargetJobTitle = record.TargetJobTitle,
                OverallScore = record.OverallScore,
                AtsScore = record.AtsScore,
                CreatedAt = record.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ResumeAnalysisHistoryDetailResponse?> GetAnalysisByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var record = await dbContext.ResumeAnalysisRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        if (record is null)
        {
            return null;
        }

        var parsed = ParseStoredResult(record.ResultJson);
        return new ResumeAnalysisHistoryDetailResponse
        {
            Id = record.Id,
            OverallScore = record.OverallScore,
            AtsScore = record.AtsScore,
            CandidateSummary = parsed?.Analysis.CandidateSummary ?? string.Empty,
            Strengths = parsed?.Analysis.Strengths ?? [],
            Weaknesses = parsed?.Analysis.Weaknesses ?? [],
            MissingKeywords = parsed?.Analysis.MissingKeywords ?? [],
            Recommendations = parsed?.Analysis.Recommendations ?? [],
            ScoreBreakdown = parsed?.Analysis.ScoreBreakdown,
            TargetJobTitle = record.TargetJobTitle,
            TargetJobDescription = parsed?.TargetJobDescription ?? string.Empty,
            Notes = parsed?.Notes,
            OriginalFileName = record.FileName,
            CreatedAt = record.CreatedAtUtc
        };
    }

    private static StoredAnalysisPayload? ParseStoredResult(string resultJson)
    {
        if (string.IsNullOrWhiteSpace(resultJson))
        {
            return null;
        }

        try
        {
            var payload = JsonSerializer.Deserialize<StoredAnalysisPayload>(resultJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (payload?.Analysis is not null)
            {
                return payload;
            }

            var legacyAnalysis = JsonSerializer.Deserialize<ResumeAnalysisResponse>(resultJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (legacyAnalysis is null)
            {
                return null;
            }

            return new StoredAnalysisPayload
            {
                Analysis = legacyAnalysis,
                TargetJobDescription = string.Empty,
                Notes = null
            };
        }
        catch
        {
            return null;
        }
    }
}
