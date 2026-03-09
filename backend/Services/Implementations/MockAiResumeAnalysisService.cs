using ResumeAnalyser.Api.Contracts.Responses;
using ResumeAnalyser.Api.Services.Interfaces;
using ResumeAnalyser.Api.Services.Models;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class MockAiResumeAnalysisService : IAiResumeAnalysisService
{
    public Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisInput input,
        CancellationToken cancellationToken = default)
    {
        var hasNotes = !string.IsNullOrWhiteSpace(input.Notes);

        var response = new ResumeAnalysisResponse
        {
            OverallScore = 74,
            AtsScore = 69,
            CandidateSummary =
                $"Candidate appears aligned to {input.TargetJobTitle}. " +
                "This is mock output until OpenAI integration is implemented.",
            Strengths =
            [
                "Clear role intent in submitted target job title",
                "Resume content includes measurable outcomes",
                "Profile demonstrates transferable technical skills"
            ],
            Weaknesses =
            [
                "Summary section could be more tailored to the role",
                "Experience bullets need stronger ATS-aligned keywords"
            ],
            MissingKeywords =
            [
                "stakeholder management",
                "cross-functional collaboration",
                "requirements prioritization"
            ],
            Recommendations =
            [
                "Rewrite your profile summary to mirror the target role language.",
                "Add role-specific keywords from the job description into experience bullets.",
                hasNotes
                    ? "Incorporate your notes into quantified achievements where relevant."
                    : "Provide optional notes to improve context-specific recommendations."
            ]
        };

        return Task.FromResult(response);
    }
}
