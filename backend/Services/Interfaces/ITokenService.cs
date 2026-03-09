using ResumeAnalyser.Api.Domain.Entities;

namespace ResumeAnalyser.Api.Services.Interfaces;

public interface ITokenService
{
    (string AccessToken, DateTime ExpiresAtUtc) CreateAccessToken(ApplicationUser user);
}
