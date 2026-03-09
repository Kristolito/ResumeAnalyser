using Microsoft.AspNetCore.Mvc;
using ResumeAnalyser.Api.Contracts.Requests;
using ResumeAnalyser.Api.Contracts.Responses;
using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Controllers;

[ApiController]
[Route("api/resume")]
public sealed class ResumeController(IResumeAnalysisService resumeAnalysisService) : ControllerBase
{
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedPdfContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf"
    };

    [HttpPost("analyse")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ResumeAnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResumeAnalysisResponse>> Analyse(
        [FromForm] ResumeAnalysisFormRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var file = request.File;
        if (file is null)
        {
            return BadRequest("A PDF file is required.");
        }

        if (file.Length == 0)
        {
            return BadRequest("The uploaded PDF file is empty.");
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return BadRequest($"The uploaded PDF exceeds the {MaxFileSizeBytes / (1024 * 1024)}MB limit.");
        }

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only PDF files are supported.");
        }

        if (!AllowedPdfContentTypes.Contains(file.ContentType))
        {
            return BadRequest("Invalid content type. Please upload a valid PDF file.");
        }

        var analysis = await resumeAnalysisService.AnalyseAsync(request, cancellationToken);
        return Ok(analysis);
    }
}
