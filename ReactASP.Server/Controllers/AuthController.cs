using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Application.DTOs.LoginUser;
using System;
using System.Net;
using Ardalis.Result;
using ReactASP.Server.DTOs.Request;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ReactASP.Server.DTOs.Response;
using ReactASP.Application.Commands.CommandEntities.Auth;
using System.Security.Cryptography;
using ReactASP.Application.Commands.CommandResults.Auth;

namespace ReactASP.Server.Controllers;
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
        _logger.LogInformation($"Register for email {request.Email} started");

        var cmd = _mapper.Map<RegisterUserCommand>(request);

        var result = await _mediator.Send(cmd, ct);
        if (!result.IsSuccess)
        {
            _logger.LogWarning($"Register failed for email {request.Email}: {string.Join("; ", result.Errors)}");
            return Problem(
                title: "Error while retrieving Register result",
                detail: string.Join("; ", result.Errors),
                statusCode: StatusCodes.Status400BadRequest);
        }

        var resp = _mapper.Map<RegisterResponse>(result.Value);
        _logger.LogInformation($"Register success for email {request.Email}");

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
        _logger.LogInformation($"Login started for email {request.Email}");

        var cmd = _mapper.Map<LoginUserCommand>(request);

        var result = await _mediator.Send(cmd, ct);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning($"Login failed for email {request.Email}: {string.Join("; ", result.Errors)}");
            return Problem(
                title: "Error while retrieving Login result",
                detail: string.Join("; ", result.Value),
                statusCode: StatusCodes.Status400BadRequest);
        }
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

        _logger.LogInformation($"Login success for email {request.Email}");

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
        _logger.LogInformation("Refresh token started");

        var cmd = _mapper.Map<RefreshTokenCommand>(refreshTokenPlain);

        var result = await _mediator.Send(cmd, ct);

        if (!result.IsSuccess)
        {
            _logger.LogWarning($"Refresh token failed {string.Join("; ", result.Errors)}");
            return Problem(
                title: "Error while retrieving Refresh result",
                detail: string.Join("; ", result.Errors),
                statusCode: StatusCodes.Status400BadRequest);
        }

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

        _logger.LogInformation("Refresh token success");
        var resp = _mapper.Map<RefreshTokenResponse>(result.Value);

        return Ok(resp);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Forgot password started");
        var cmd = _mapper.Map<ForgotPasswordCommand>(request);
        var result = await _mediator.Send(cmd, ct);

        if (!result.IsSuccess)
        {
            _logger.LogWarning($"Forgot password failed {string.Join("; ", result.Errors)}");
            return Problem(
                title: "Error while retrieving ForgotPassword result",
                detail: string.Join("; ", result.Errors),
                statusCode: StatusCodes.Status400BadRequest);
        }

        _logger.LogInformation("Forgot password ended");
        var resp = _mapper.Map<ForgotPasswordResponse>(result);
        return Ok(resp);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        var cmd = _mapper.Map<ResetPasswordCommand>(request);

        var result = await _mediator.Send(cmd, ct);

        if (!result.IsSuccess)
        {
            return Problem(
                title: "Error while retrieving ResetPassword result",
                detail: string.Join("; ", result.Errors),
                statusCode: StatusCodes.Status400BadRequest);
        }

        var resp = _mapper.Map<ResetPasswordResponse>(result);
        return Ok(resp);
    }
}