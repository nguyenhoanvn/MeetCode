using Ardalis.Result;
using AutoMapper;
using MediatR;
using MeetCode.Application.DTOs.Response.Auth;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using MeetCode.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Auth
{
    public class LoginAdminQueryHandler : IRequestHandler<LoginAdminQuery, Result<LoginAdminQueryResult>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<LoginUserQueryHandler> _logger;
        private readonly IAuthService _authService;

        public LoginAdminQueryHandler(
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

        public async Task<Result<LoginAdminQueryResult>> Handle(LoginAdminQuery request, CancellationToken ct)
        {

            _logger.LogInformation($"Login handler started for user {request.Email}");
            // Verify email
            var email = request.Email.Trim().ToLowerInvariant();
            var userResult = await _userService.FindUserByEmailAsync(email, ct);

            if (!userResult.IsSuccess)
            {
                _logger.LogInformation($"Login failed because email not match");
                return Result.Invalid(new ValidationError(nameof(email), "Invalid credentials"));
            }

            var user = userResult.Value;

            // Verify password
            var isPasswordMatch = _userService.IsPasswordMatch(request.Password, user.PasswordHash);

            if (!isPasswordMatch)
            {
                _logger.LogInformation($"Login failed because password does not match");
                return Result.Invalid(new ValidationError(nameof(request.Password), "Invalid credentials"));
            }

            if (user.Role == UserRole.User)
            {
                _logger.LogWarning("Logged in user is not administrator priviledge user");
                return Result.Invalid(new ValidationError("Role", "This account does not have access to admin features"));
            }

            string accessToken = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            string refreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.CreateRefreshTokenAsync(user.UserId, refreshToken, ct);
            await _authService.UpdateLoginTime(user.UserId, ct);

            _logger.LogInformation($"Login completed for user with email: {email}");

            var result = new LoginAdminQueryResult(user, accessToken, refreshToken);

            return Result.Success(result);
        }
    }
}
