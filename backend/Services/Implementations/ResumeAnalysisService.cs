using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class ResumeAnalysisService : IResumeAnalysisService
{
    public async Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return new ResumeAnalysisResponse
        {
            OverallScore = 72,
            AtsScore = 68,
            CandidateSummary = "Mock analysis response",
            Strengths = ["Good technical skills"],
            Weaknesses = ["Needs quantified achievements"],
            MissingKeywords = ["Microservices", "CI/CD"],
            Recommendations = ["Add measurable achievements"]
        };
    }
}
