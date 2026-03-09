using System.Text.RegularExpressions;
using ResumeAnalyser.Api.Services.Exceptions;
using ResumeAnalyser.Api.Services.Interfaces;
using UglyToad.PdfPig;

namespace ResumeAnalyser.Api.Services.Implementations;

public sealed class PdfTextExtractor : IPdfTextExtractor
{
    private static readonly Regex MultiSpaceRegex = new(@"[ \t]{2,}", RegexOptions.Compiled);
    private static readonly Regex MultiLineRegex = new(@"(\r?\n){3,}", RegexOptions.Compiled);

    public async Task<string> ExtractTextAsync(IFormFile pdfFile, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var uploadedFileStream = pdfFile.OpenReadStream();
            await using var memoryStream = new MemoryStream();
            await uploadedFileStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            using var document = PdfDocument.Open(memoryStream);
            var pageTexts = new List<string>();

            foreach (var page in document.GetPages())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(page.Text))
                {
                    continue;
                }

                pageTexts.Add(page.Text.Trim());
            }

            var combinedText = string.Join(Environment.NewLine + Environment.NewLine, pageTexts).Trim();
            combinedText = MultiSpaceRegex.Replace(combinedText, " ");
            combinedText = MultiLineRegex.Replace(combinedText, Environment.NewLine + Environment.NewLine);

            if (string.IsNullOrWhiteSpace(combinedText))
            {
                throw new PdfTextExtractionException(
                    "The uploaded PDF was read successfully, but no usable text content was found.");
            }

            return combinedText;
        }
        catch (PdfTextExtractionException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new PdfTextExtractionException(
                "Unable to read the uploaded PDF. Please upload a valid text-based PDF file.",
                exception);
        }
    }
}
