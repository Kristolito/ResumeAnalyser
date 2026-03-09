using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class MockPdfTextExtractor : IPdfTextExtractor
{
    public Task<string> ExtractTextAsync(IFormFile pdfFile, CancellationToken cancellationToken = default)
    {
        var mockText =
            $"Mock extracted text for '{pdfFile.FileName}'. " +
            "Replace MockPdfTextExtractor with a real PDF parser in the next phase.";

        return Task.FromResult(mockText);
    }
}
