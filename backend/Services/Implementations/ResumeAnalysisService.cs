using System.Text.Json;
using ResumeAnalyser.Api.Contracts.Requests;
using ResumeAnalyser.Api.Contracts.Responses;
using ResumeAnalyser.Api.Data;
using ResumeAnalyser.Api.Domain.Entities;
using ResumeAnalyser.Api.Services.Interfaces;
using ResumeAnalyser.Api.Services.Models;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class ResumeAnalysisService(
    IPdfTextExtractor pdfTextExtractor,
    IAiResumeAnalysisService aiResumeAnalysisService,
    AppDbContext dbContext) : IResumeAnalysisService
{
    public async Task<ResumeAnalysisResponse> AnalyseAsync(
        ResumeAnalysisFormRequest request,
        CancellationToken cancellationToken = default)
    {
        var file = request.File!;
        var resumeText = await pdfTextExtractor.ExtractTextAsync(file, cancellationToken);

        var analysisInput = new ResumeAnalysisInput
        {
            FileName = file.FileName,
            ResumeText = resumeText,
            TargetJobTitle = request.TargetJobTitle.Trim(),
            TargetJobDescription = request.TargetJobDescription.Trim(),
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim()
        };

        var response = await aiResumeAnalysisService.AnalyseAsync(analysisInput, cancellationToken);

        var record = new ResumeAnalysisRecord
        {
            FileName = file.FileName,
            TargetJobTitle = analysisInput.TargetJobTitle,
            OverallScore = response.OverallScore,
            AtsScore = response.AtsScore,
            ResultJson = JsonSerializer.Serialize(response),
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.ResumeAnalysisRecords.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}
