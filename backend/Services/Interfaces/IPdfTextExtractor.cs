namespace ResumeAnalyser.Api.Services.Interfaces;

public interface IPdfTextExtractor
{
    Task<string> ExtractTextAsync(IFormFile pdfFile, CancellationToken cancellationToken = default);
}
