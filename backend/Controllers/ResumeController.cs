using Microsoft.AspNetCore.Mvc;
using ResumeAnalyser.Api.Infrastructure.FileValidation;
using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Models.History;
using ResumeAnalyser.Api.Services.Exceptions;
using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Controllers;

[ApiController]
[Route("api/resume")]
public sealed class ResumeController(
    IResumeAnalysisService resumeAnalysisService,
    IResumeAnalysisHistoryService resumeAnalysisHistoryService,
    IPdfFileValidator pdfFileValidator) : ControllerBase
{
    [HttpPost("analyse")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ResumeAnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResumeAnalysisResponse>> Analyse(
        [FromForm] ResumeAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var fileValidationError = pdfFileValidator.Validate(request.File);
        if (!string.IsNullOrWhiteSpace(fileValidationError))
        {
            return BadRequest(fileValidationError);
        }

        try
        {
            var analysis = await resumeAnalysisService.AnalyseAsync(request, cancellationToken);
            return Ok(analysis);
        }
        catch (PdfTextExtractionException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("analyses")]
    [ProducesResponseType(typeof(IReadOnlyList<ResumeAnalysisHistoryItemResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ResumeAnalysisHistoryItemResponse>>> GetAnalyses(
        CancellationToken cancellationToken)
    {
        var analyses = await resumeAnalysisHistoryService.GetAnalysesAsync(cancellationToken);
        return Ok(analyses);
    }

    [HttpGet("analyses/{id:guid}")]
    [ProducesResponseType(typeof(ResumeAnalysisHistoryDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ResumeAnalysisHistoryDetailResponse>> GetAnalysisById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var analysis = await resumeAnalysisHistoryService.GetAnalysisByIdAsync(id, cancellationToken);
        if (analysis is null)
        {
            return NotFound();
        }

        return Ok(analysis);
    }
}
