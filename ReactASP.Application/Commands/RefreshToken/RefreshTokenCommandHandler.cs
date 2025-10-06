using MediatR;
using ReactASP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ReactASP.Application.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResult>>
    {
        private readonly int RT_EXPIRE_TIME = 30;

        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            ILogger<RefreshTokenCommandHandler> logger)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        } 

        public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            try
            {
                // Find refresh token
                var hashedRefreshToken = _tokenService.HashToken(request.refreshToken);
                var refreshTokenEntity = await _refreshTokenRepository.GetByToken(hashedRefreshToken, ct);
                if (refreshTokenEntity is null || !await _tokenService.ValidateRefreshToken(request.refreshToken, ct))
                {
                    _logger.LogWarning($"Invalid refresh token received: {request.refreshToken}");
                    return Result.Unauthorized("Refresh token is invalid");
                }

                // Find user with refresh token
                var user = await _userRepository.FindUserAsync(refreshTokenEntity.UserId, ct);
                if (user is null)
                {
                    _logger.LogWarning($"Invalid user with refresh token: {request.refreshToken}");
                    return Result.Unauthorized("User with refresh token not found");
                }

                // Generate tokens
                var jwt = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                var newRefreshTokenEntity = new Domain.Entities.RefreshToken
                {
                    UserId = user.UserId,
                    TokenHash = _tokenService.HashToken(newRefreshToken),
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(RT_EXPIRE_TIME)
                };

                await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                _logger.LogInformation($"Issued new refresh token for user with Id: {user.UserId}");

                return Result.Success(new RefreshTokenResult(jwt, newRefreshToken));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occured: {ex.Message}");
                return Result.Error("An unexpected error occurred while refreshing token");
            }
            

            
        }
    }
}
