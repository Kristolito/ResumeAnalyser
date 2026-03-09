using ResumeAnalyser.Api.Contracts.Responses;
using ResumeAnalyser.Api.Services.Models;

namespace ResumeAnalyser.Api.Services.Interfaces;

public interface IAiResumeAnalysisService
{
    Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisInput input,
        CancellationToken cancellationToken = default);
}
