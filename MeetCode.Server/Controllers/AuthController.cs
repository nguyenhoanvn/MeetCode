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
using Ardalis.Result.AspNetCore;

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
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<RegisterResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<RegisterUserCommand>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<RegisterResponse>(value));

        return resp;
    }

    [HttpPost("login")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<LoginUserQuery>(request);

        var result = await _mediator.Send(cmd, ct);

        // Happy case
        if (result.Value.IsSuccessful == true)
        {
            HttpContext.Response.Cookies.Append(
            "accessToken",
            result.Value.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });
            HttpContext.Response.Cookies.Append(
                "refreshToken",
                result.Value.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }

        var resp = result.Map(value => _mapper.Map<LoginResponse>(value));

        return resp;
    }

    [HttpGet("refresh")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Error)]
    public async Task<Result<RefreshTokenResponse>> Refresh(CancellationToken ct)
    { 
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenPlain) ||
                string.IsNullOrWhiteSpace(refreshTokenPlain))
        {
            _logger.LogWarning("Refresh token failed because of mission token");
            return Result.Success();
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

        var resp = result.Map(value => _mapper.Map<RefreshTokenResponse>(value));

        return resp;
    }

    [HttpPost("forgot-password")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<ForgotPasswordQuery>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<ForgotPasswordResponse>(value));
        return resp;
    }

    [HttpPost("reset-password")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Unauthorized)]
    public async Task<Result<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<ResetPasswordCommand>(request);

        var result = await _mediator.Send(cmd, ct);

        var resp = result.Map(value => _mapper.Map<ResetPasswordResponse>(value));

        return resp;
    }
}