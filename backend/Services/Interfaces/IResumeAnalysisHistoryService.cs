using ResumeAnalyser.Api.Models.History;

namespace ResumeAnalyser.Api.Services.Interfaces;

public interface IResumeAnalysisHistoryService
{
    Task<IReadOnlyList<ResumeAnalysisHistoryItemResponse>> GetAnalysesAsync(
        CancellationToken cancellationToken = default);

    Task<ResumeAnalysisHistoryDetailResponse?> GetAnalysisByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
