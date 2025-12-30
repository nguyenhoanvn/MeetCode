using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.DTOs.Response.Auth;
using MeetCode.Application.Interfaces;
using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Auth
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {

        private readonly ITokenService _tokenService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;
        private readonly IUserService _userService;

        public RefreshTokenCommandHandler(
            ITokenService tokenService,
            ILogger<RefreshTokenCommandHandler> logger,
            IUserService userService)
        {
            _tokenService = tokenService;
            _logger = logger;
            _userService = userService;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {

            _logger.LogInformation("Issue refresh token handler started");
            // Find refresh token 
            var refreshTokenResult = await _tokenService.FindRefreshTokenByTokenAsync(request.PlainRefreshToken, ct);

            if (!refreshTokenResult.IsSuccess)
            {
                _logger.LogWarning("Validation failed for token {Token}. Errors: {Error}",
                    request.PlainRefreshToken,
                    string.Join("\n", refreshTokenResult.ValidationErrors.Select(e => $"{e.Identifier}: {e.ErrorMessage}"))
                );
                return Result.Invalid(refreshTokenResult.ValidationErrors);
            }

            var refreshToken = refreshTokenResult.Value;

            // Find user with refresh token
            var user = await _userService.FindUserAsync(refreshToken.UserId, ct);

            // Generate tokens
            var jwt = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenCreate = await _tokenService.CreateRefreshTokenAsync(user.UserId, newRefreshToken, ct);

            _logger.LogInformation($"Issued new refresh token for user with Id: {user.UserId}");

            var resp = new RefreshTokenResponse(jwt, newRefreshToken);

            return Result.Success(resp);

        }
    }
}
