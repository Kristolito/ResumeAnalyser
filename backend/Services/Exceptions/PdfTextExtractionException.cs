namespace ResumeAnalyser.Api.Services.Exceptions;

public sealed class PdfTextExtractionException(string message, Exception? innerException = null)
    : Exception(message, innerException);
