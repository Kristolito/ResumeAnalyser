using Microsoft.AspNetCore.Mvc;
using ResumeAnalyser.Api.Infrastructure.FileValidation;
using ResumeAnalyser.Api.Models;
using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Controllers;

[ApiController]
[Route("api/resume")]
public sealed class ResumeController(
    IResumeAnalysisService resumeAnalysisService,
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

        var analysis = await resumeAnalysisService.AnalyseAsync(request, cancellationToken);
        return Ok(analysis);
    }
}
