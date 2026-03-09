namespace ResumeAnalyser.Api.Infrastructure.FileValidation;

public interface IPdfFileValidator
{
    string? Validate(IFormFile? file);
}

public sealed class PdfFileValidator : IPdfFileValidator
{
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedPdfContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf"
    };

    public string? Validate(IFormFile? file)
    {
        if (file is null)
        {
            return "A PDF file is required.";
        }

        if (file.Length == 0)
        {
            return "The uploaded PDF file is empty.";
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return $"The uploaded PDF exceeds the {MaxFileSizeBytes / (1024 * 1024)}MB limit.";
        }

        var extension = Path.GetExtension(file.FileName);
        if (!string.Equals(extension, ".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return "Only PDF files are supported.";
        }

        if (!AllowedPdfContentTypes.Contains(file.ContentType))
        {
            return "Invalid content type. Please upload a valid PDF file.";
        }

        return null;
    }
}
