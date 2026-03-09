namespace ResumeAnalyser.Api.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "ResumeAnalyser";

    public string Audience { get; set; } = "ResumeAnalyserClient";

    public string SigningKey { get; set; } = "replace-with-strong-random-key-at-least-32-chars";

    public int ExpiresInMinutes { get; set; } = 120;
}
