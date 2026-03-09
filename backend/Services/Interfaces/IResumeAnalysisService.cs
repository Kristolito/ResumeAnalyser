using ResumeAnalyser.Api.Contracts.Requests;
using ResumeAnalyser.Api.Contracts.Responses;

namespace ResumeAnalyser.Api.Services.Interfaces;

public interface IResumeAnalysisService
{
    Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisFormRequest request,
        CancellationToken cancellationToken = default);
}
