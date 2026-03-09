using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class ResumeAnalysisService(
    IPdfTextExtractor pdfTextExtractor,
    ILogger<ResumeAnalysisService> logger,
    IHostEnvironment hostEnvironment) : IResumeAnalysisService
{
    public async Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        var extractedResumeText = await pdfTextExtractor.ExtractTextAsync(request.File!, cancellationToken);
        var textPreview = extractedResumeText.Length > 320
            ? extractedResumeText[..320] + "..."
            : extractedResumeText;

        logger.LogInformation(
            "PDF text extraction completed for {FileName}. Extracted length: {ExtractedLength}. Preview: {Preview}",
            request.File?.FileName,
            extractedResumeText.Length,
            textPreview);

        var response = new ResumeAnalysisResponse
        {
            OverallScore = 72,
            AtsScore = 68,
            CandidateSummary = "Mock analysis response",
            Strengths = ["Good technical skills"],
            Weaknesses = ["Needs quantified achievements"],
            MissingKeywords = ["Microservices", "CI/CD"],
            Recommendations = ["Add measurable achievements"]
        };

        if (hostEnvironment.IsDevelopment())
        {
            response.DebugExtractedTextPreview = textPreview;
        }

        return response;
    }
}
