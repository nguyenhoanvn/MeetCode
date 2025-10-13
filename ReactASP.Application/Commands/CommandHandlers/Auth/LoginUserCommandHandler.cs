using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Application.Interfaces;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Application.Commands.CommandEntities.Auth;
using ReactASP.Application.Commands.CommandResults.Auth;

namespace ReactASP.Application.Commands.CommandHandlers.Auth
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResult>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(
            ITokenService tokenService,
            IUserService userService,
            ILogger<LoginUserCommandHandler> logger)
        {
            _tokenService = tokenService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<Result<LoginUserResult>> Handle(LoginUserCommand request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation($"Login handler started for user {request.Email}");
                // Verify email
                var email = request.Email.Trim().ToLowerInvariant();
                var user = await _userService.FindUserByEmailAsync(email, ct);

                // Verify password
                var isPasswordMatch = _userService.IsPasswordMatch(request.Password, user.PasswordHash);

                if (!isPasswordMatch)
                {
                    _logger.LogWarning($"Login failed because password does not match");
                    return Result.Invalid(new ValidationError
                    {
                        Identifier = nameof(request.Password),
                        ErrorMessage = "Invalid email"
                    });
                }

                string accessToken = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
                string refreshToken = _tokenService.GenerateRefreshToken();

                await _tokenService.CreateRefreshTokenAsync(user.UserId, refreshToken, ct);
                await _userService.UpdateLoginTime(user.UserId, ct);

                _logger.LogInformation($"Login completed for user with email: {email}");

                return Result.Success(new LoginUserResult(accessToken, refreshToken, user.DisplayName, user.Role));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return Result.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occured while login: {ex.Message}");
                return Result.Error("An exception occured while login");
            }
        }
    }
}
