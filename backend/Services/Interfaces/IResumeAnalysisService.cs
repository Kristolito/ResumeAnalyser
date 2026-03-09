using ResumeAnalyser.Api.Models;

namespace ResumeAnalyser.Api.Services.Interfaces;

public interface IResumeAnalysisService
{
    Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisRequest request,
        CancellationToken cancellationToken = default);
}
