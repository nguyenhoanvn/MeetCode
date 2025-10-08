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
using ReactASP.Server.DTOs.RefreshToken;

namespace ReactASP.Server.Controllers;
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var cmd = new RegisterUserCommand(request.Email, request.DisplayName, request.Password);

        try
        {
            var result = await _mediator.Send(cmd, ct);

            var resp = new RegisterResponse
            {
                UserId = result.Value.userId,
                Email = result.Value.email,
                DisplayName = result.Value.displayName,
                Role = result.Value.role
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

    [HttpGet("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var cmd = new LoginUserCommand(request.Email, request.Password);

        try
        {
            var result = await _mediator.Send(cmd, ct);
            var resp = new LoginResponse
            {
                AccessToken = result.Value.accessToken,
                RefreshToken = result.Value.refreshToken,
                DisplayName = result.Value.displayName,
                Role = result.Value.refreshToken
            };

            HttpContext.Response.Cookies.Append(
                "accessToken",
                resp.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
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

    [HttpGet("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {

        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) ||
                string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized("Refresh token is invalid");
        }

        var cmd = new RefreshTokenCommand(refreshToken);

        var result = await _mediator.Send(cmd, ct);

        if (result.IsUnauthorized())
        {
            return Unauthorized();
        }

        if (result.Status != ResultStatus.Ok)
        {
            return BadRequest(result.Errors);
        }

        HttpContext.Response.Cookies.Append(
            "accessToken",
            result.Value.jwt,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

        var resp = new RefreshTokenResponse {
            AccessToken = result.Value.jwt,
            NewRefreshToken = result.Value.refreshToken
        };

        return Ok(resp);
    }
}