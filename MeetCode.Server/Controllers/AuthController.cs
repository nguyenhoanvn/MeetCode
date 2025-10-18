using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Ardalis.Result;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Server.DTOs.Response.Auth;
using MeetCode.Server.DTOs.Request.Auth;

namespace MeetCode.Server.Controllers;
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;
    public AuthController(
        ISender mediator,
        IMapper mapper,
        ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        if (request == null)
        {
            _logger.LogWarning($"Register failed because request is null");
            return BadRequest("Invalid request body");
        }

        var cmd = _mapper.Map<RegisterUserCommand>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = _mapper.Map<RegisterResponse>(result.Value);
        return CreatedAtAction(nameof(Register), new { Id = resp.UserId }, resp);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (request == null)
        {
            return BadRequest("Invalid Login request body");
        }

        var cmd = _mapper.Map<LoginUserQuery>(request);

        var result = await _mediator.Send(cmd, ct);
        
        var resp = _mapper.Map<LoginResponse>(result.Value);

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
        HttpContext.Response.Cookies.Append(
            "refreshToken",
            resp.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

        return Ok(resp);
    }

    [Authorize(Roles = "user")]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    { 
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenPlain) ||
                string.IsNullOrWhiteSpace(refreshTokenPlain))
        {
            _logger.LogWarning("Refresh token failed because of mission token");
            return Unauthorized("Missing refresh token");
        }

        var cmd = _mapper.Map<RefreshTokenCommand>(refreshTokenPlain);

        var result = await _mediator.Send(cmd, ct);

        HttpContext.Response.Cookies.Append(
            "accessToken",
            result.Value.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

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

        var resp = _mapper.Map<RefreshTokenResponse>(result.Value);

        return Ok(resp);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Forgot password started");
        var cmd = _mapper.Map<ForgotPasswordQuery>(request);
        var result = await _mediator.Send(cmd, ct);

        _logger.LogInformation("Forgot password ended");
        var resp = _mapper.Map<ForgotPasswordResponse>(result.Value);
        return Ok(resp);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<ResetPasswordCommand>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = _mapper.Map<ResetPasswordResponse>(result.Value);
        return Ok(resp);
    }
}