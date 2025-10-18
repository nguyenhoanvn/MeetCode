using MediatR;
using MeetCode.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;

namespace MeetCode.Application.Commands.CommandHandlers.Auth
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResult>>
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

        public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {

            _logger.LogInformation("Issue refresh token handler started");
            // Find refresh token 
            var refreshToken = await _tokenService.FindRefreshTokenByTokenAsync(request.PlainRefreshToken, ct);

            // Find user with refresh token
            var user = await _userService.FindUserAsync(refreshToken.UserId, ct);

            // Generate tokens
            var jwt = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenCreate = await _tokenService.CreateRefreshTokenAsync(user.UserId, newRefreshToken, ct);

            _logger.LogInformation($"Issued new refresh token for user with Id: {user.UserId}");

            return Result.Success(new RefreshTokenResult(jwt, refreshTokenCreate.TokenHash));

        }
    }
}
