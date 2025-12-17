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
using MeetCode.Application.DTOs.Response.Auth;
using AutoMapper;

namespace MeetCode.Application.Queries.QueryHandlers.Auth
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<LoginResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<LoginUserQueryHandler> _logger;
        private readonly IAuthService _authService;
        public LoginUserQueryHandler(
            ITokenService tokenService,
            IUserService userService,
            IMapper mapper,
            ILogger<LoginUserQueryHandler> logger,
            IAuthService authService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
        }

        public async Task<Result<LoginResponse>> Handle(LoginUserQuery request, CancellationToken ct)
        {

            _logger.LogInformation($"Login handler started for user {request.Email}");
            // Verify email
            var email = request.Email.Trim().ToLowerInvariant();
            var userResult = await _userService.FindUserByEmailAsync(email, ct);

            if (!userResult.IsSuccess)
            {
                _logger.LogInformation($"Login failed because email not match");
                return Result.Invalid(new ValidationError(nameof(email), "Email is invalid"));
            }

            var user = userResult.Value;

            // Verify password
            var isPasswordMatch = _userService.IsPasswordMatch(request.Password, user.PasswordHash);

            if (!isPasswordMatch)
            {
                _logger.LogInformation($"Login failed because password does not match");
                return Result.Invalid(new ValidationError(nameof(request.Password), "Password is invalid"));
            }

            string accessToken = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            string refreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.CreateRefreshTokenAsync(user.UserId, refreshToken, ct);
            await _authService.UpdateLoginTime(user.UserId, ct);

            _logger.LogInformation($"Login completed for user with email: {email}");

            var result = new LoginUserQueryResult(accessToken, refreshToken, user.DisplayName, user.Role);
            var resp = _mapper.Map<LoginResponse>(result);

            return Result.Success(resp);
        }
    }
}
