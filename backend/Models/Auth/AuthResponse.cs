namespace ResumeAnalyser.Api.Models.Auth;

public sealed class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public UserProfileResponse User { get; set; } = new();
}
