using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Application.Interfaces;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ReactASP.Application.Commands.LoginUser
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResult>>
    {
        private readonly int RT_EXPIRE_TIME = 30;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(IUserRepository userRepository, 
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ILogger<LoginUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<LoginUserResult>> Handle(LoginUserCommand request, CancellationToken ct)
        {
            try
            {
                // Create new refresh token
                var email = request.Email.Trim().ToLowerInvariant();

                var user = await _userRepository.GetUserByEmailWithTokensAsync(email, ct);
                if (user is null)
                {
                    _logger.LogWarning($"Cannot find the user with email: {email}");
                    return Result.Invalid(new ValidationError
                    {
                        Identifier = nameof(email),
                        ErrorMessage = "Invalid email"
                    });
                }

                var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
                if (!ok)
                {
                    _logger.LogWarning($"Wrong password");
                    return Result.Invalid(new ValidationError
                    {
                        Identifier = nameof(request.Password),
                        ErrorMessage = "Invalid email"
                    });
                }

                string accessToken = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
                string refreshToken = _tokenService.GenerateRefreshToken();

                Domain.Entities.RefreshToken refreshTokenEntity = new Domain.Entities.RefreshToken
                {
                    UserId = user.UserId,
                    TokenHash = _tokenService.HashToken(refreshToken),
                    ExpiresAt = DateTimeOffset.UtcNow.AddDays(RT_EXPIRE_TIME),
                    CreatedAt = DateTimeOffset.UtcNow
                };

                // Revoke old refresh token
                var oldRefreshTokenArray = user.RefreshTokens.Where(rt => !rt.IsRevoked && rt.ExpiresAt > DateTimeOffset.UtcNow).ToList(); ;
                foreach (var oldRefreshToken in oldRefreshTokenArray)
                {
                    oldRefreshToken.IsRevoked = true;
                }

                // Attach refresh token
                await _refreshTokenRepository.AddAsync(refreshTokenEntity, ct);
                await _unitOfWork.SaveChangesAsync(ct);
                _logger.LogInformation($"Login in completed for user with email: {email}");

                return Result.Success(new LoginUserResult(accessToken, refreshToken, user.DisplayName, user.Role));
            } catch (Exception ex)
            {
                _logger.LogError($"An exception occured while login: {ex.Message}");
                return Result.Error("An exception occured while login");
            }
        }
    }
}
