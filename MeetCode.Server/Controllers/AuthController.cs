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
using MeetCode.Application.DTOs.Response.Auth;
using MeetCode.Application.DTOs.Request.Auth;
using Ardalis.Result.AspNetCore;

namespace MeetCode.Server.Controllers;
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly ILogger<AuthController> _logger;
    public AuthController(
        ISender mediator,
        ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<RegisterResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var cmd = new RegisterUserCommand(
                request.Email,
                request.DisplayName,
                request.Password
            );

        var resp = await _mediator.Send(cmd, ct);

        return resp;
    }

    [HttpPost("login")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var cmd = new LoginUserQuery(
                request.Email,
                request.Password
            );

        var resp = await _mediator.Send(cmd, ct);

        // Happy case
        if (resp.IsSuccess == true)
        {
            HttpContext.Response.Cookies.Append(
            "accessToken",
            resp.Value.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30)
            });
            HttpContext.Response.Cookies.Append(
                "refreshToken",
                resp.Value.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
        }

        return resp;
    }

    [Authorize(Roles = "User")]
    [HttpPost("logout")]
    [TranslateResultToActionResult]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<Result<string>> Logout(CancellationToken ct)
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null)
        {
            return Result.Unauthorized("Not logged in");
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var cmd = new LogoutCommand(userId);

        var result = await _mediator.Send(cmd, ct);

        Response.Cookies.Delete("accessToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Result.Success("Logout successful");
    }

    [HttpGet("refresh")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(RefreshTokenResult), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Error)]
    public async Task<Result<RefreshTokenResult>> Refresh(CancellationToken ct)
    { 
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshTokenPlain) ||
                string.IsNullOrWhiteSpace(refreshTokenPlain))
        {
            _logger.LogWarning("Refresh token failed because of mission token");
            return Result.Success();
        }

        var cmd = new RefreshTokenCommand(refreshTokenPlain);

        var resp = await _mediator.Send(cmd, ct);

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
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        var cmd = new ForgotPasswordQuery(
                request.Email
            );

        var resp = await _mediator.Send(cmd, ct);

        return resp;
    }

    [HttpPost("verify-otp")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(VerifyResetPasswordOTPResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Error)]
    public async Task<Result<VerifyResetPasswordOTPResponse>> VerifyOTP([FromBody] VerifyResetPasswordOTPRequest request, CancellationToken ct)
    {
        var cmd = new VerifyResetPasswordOTPQuery(
                request.Email,
                request.Code
            );

        var resp = await _mediator.Send(cmd, ct);

        return resp;
    }

    [HttpPost("reset-password")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Unauthorized)]
    public async Task<Result<ResetPasswordResponse>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var cmd = new ResetPasswordCommand(
                request.Email,
                request.NewPassword
            );

        var resp = await _mediator.Send(cmd, ct);

        return resp;
    }
}