using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeAnalyser.Api.Domain.Entities;
using ResumeAnalyser.Api.Models.Auth;
using ResumeAnalyser.Api.Services.Interfaces;

namespace ResumeAnalyser.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
        {
            return BadRequest("An account with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(error => error.Description));
            return BadRequest(errors);
        }

        var (accessToken, expiresAtUtc) = tokenService.CreateAccessToken(user);
        return Created(string.Empty, new AuthResponse
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAtUtc,
            User = new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty
            }
        });
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        var (accessToken, expiresAtUtc) = tokenService.CreateAccessToken(user);
        return Ok(new AuthResponse
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expiresAtUtc,
            User = new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty
            }
        });
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileResponse>> Me()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new UserProfileResponse
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty
        });
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        return Ok(new { message = "Logged out." });
    }
}
