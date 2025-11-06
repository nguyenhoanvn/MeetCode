using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using Microsoft.Extensions.Logging;
using MeetCode.Application.Commands.CommandHandlers.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;

namespace MeetCode.Application.Queries.QueryHandlers.Auth
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<LoginUserQueryResult>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<LoginUserQueryHandler> _logger;
        private readonly IAuthService _authService;
        public LoginUserQueryHandler(
            ITokenService tokenService,
            IUserService userService,
            ILogger<LoginUserQueryHandler> logger,
            IAuthService authService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _logger = logger;
            _authService = authService;
        }

        public async Task<Result<LoginUserQueryResult>> Handle(LoginUserQuery request, CancellationToken ct)
        {

            _logger.LogInformation($"Login handler started for user {request.Email}");
            // Verify email
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _userService.FindUserByEmailAsync(email, ct);

            if (user == null)
            {
                _logger.LogInformation($"Login failed because email not match");
                var result = new LoginUserQueryResult("hehe", "hehe", "hehe", "hehe", false, "Invalid credentials");
                return Result.Success(result);
            }

            // Verify password
            var isPasswordMatch = _userService.IsPasswordMatch(request.Password, user.PasswordHash);

            if (!isPasswordMatch)
            {
                _logger.LogWarning($"Login failed because password does not match");
                var result = new LoginUserQueryResult("hehe", "hehe", "hehe", "hehe", false, "Invalid credentials");
                return Result.Success(result);
            }

            string accessToken = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            string refreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.CreateRefreshTokenAsync(user.UserId, refreshToken, ct);
            await _authService.UpdateLoginTime(user.UserId, ct);

            _logger.LogInformation($"Login completed for user with email: {email}");

            return Result.Success(new LoginUserQueryResult(accessToken, refreshToken, user.DisplayName, user.Role, true, ""));

        }
    }
}
