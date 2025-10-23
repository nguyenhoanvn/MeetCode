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
    public async Task<Result<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {

        var cmd = _mapper.Map<LoginUserQuery>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<LoginResponse>(value));

        HttpContext.Response.Cookies.Append(
            "accessToken",
            resp.Value.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });
        HttpContext.Response.Cookies.Append(
            "refreshToken",
            resp.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

        return resp;
    }

    [Authorize(Roles = "user")]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<RefreshTokenResponse>> Refresh(CancellationToken ct)
    { 
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenPlain) ||
                string.IsNullOrWhiteSpace(refreshTokenPlain))
        {
            _logger.LogWarning("Refresh token failed because of mission token");
            return Result.Unauthorized("Missing refresh token");
        }

        var cmd = _mapper.Map<RefreshTokenCommand>(refreshTokenPlain);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<RefreshTokenResponse>(value));

        HttpContext.Response.Cookies.Append(
            "accessToken",
            resp.Value.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });

        HttpContext.Response.Cookies.Append(
            "refreshToken",
            resp.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

        return resp;
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<ForgotPasswordQuery>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<ForgotPasswordResponse>(value));
        return resp;
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<ResetPasswordCommand>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<ResetPasswordResponse>(value));

        return resp;
    }
}