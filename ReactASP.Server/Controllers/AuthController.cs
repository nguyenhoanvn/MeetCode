using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Application.Commands.LoginUser;
using ReactASP.Application.Commands.RefreshToken;
using ReactASP.Application.Commands.RegisterUser;
using ReactASP.Application.DTOs.LoginUser;
using ReactASP.Application.DTOs.RefreshToken;
using ReactASP.Application.DTOs.RegisterUser;
using System;
using System.Net;
using Ardalis.Result;

namespace ReactASP.Server.Controllers;
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _meditator;
    public AuthController(ISender meditator)
    {
        _meditator = meditator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var cmd = new RegisterUserCommand(request.Email, request.DisplayName, request.Password);

        try
        {
            var result = await _meditator.Send(cmd, ct);

            var resp = new RegisterResponse
            {
                UserId = result.UserId,
                Email = result.Email,
                DisplayName = result.DisplayName,
                Role = result.Role
            };

            return CreatedAtAction(nameof(Register), new { Id = resp.UserId }, resp);
        } catch (InvalidOperationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Registration Error",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var cmd = new LoginUserCommand(request.Email, request.Password);

        try
        {
            var result = await _meditator.Send(cmd, ct);
            var resp = new LoginResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                DisplayName = result.DisplayName,
                Role = result.Role
            };

            HttpContext.Response.Cookies.Append(
                "refreshToken",
                resp.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

            return Ok(resp);
        } catch (InvalidOperationException ex)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Login Error",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) ||
                string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized("Refresh token is invalid");
        }

        var cmd = new RefreshTokenCommand(refreshToken);

        var result = await _meditator.Send(cmd, ct);

        if (result.IsUnauthorized())
        {
            return Unauthorized();
        }

        if (result.Status != ResultStatus.Ok)
        {
            return BadRequest(result.Errors);
        }

        HttpContext.Response.Cookies.Append(
            "refreshToken",
            result.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

        var resp = new RefreshTokenResponse {
            AccessToken = result.Value.Jwt,
        };

        return Ok(resp);
    }
}