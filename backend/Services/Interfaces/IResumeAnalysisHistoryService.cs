using ResumeAnalyser.Api.Models.History;

namespace ResumeAnalyser.Api.Services.Interfaces;

public interface IResumeAnalysisHistoryService
{
    Task<IReadOnlyList<ResumeAnalysisHistoryItemResponse>> GetAnalysesAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<ResumeAnalysisHistoryDetailResponse?> GetAnalysisByIdAsync(
        string userId,
        Guid id,
        CancellationToken cancellationToken = default);
}
