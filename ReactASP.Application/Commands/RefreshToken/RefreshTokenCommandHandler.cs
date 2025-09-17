using MediatR;
using ReactASP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace ReactASP.Application.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        } 

        public async Task<Result<RefreshTokenResult>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var hashedRefreshToken = _tokenService.HashToken(request.refreshToken);

            var refreshTokenEntity = await _refreshTokenRepository.GetByToken(hashedRefreshToken, ct);
            if (refreshTokenEntity is null || !await _tokenService.ValidateRefreshToken(request.refreshToken, ct))
            {
                throw new UnauthorizedAccessException("Refresh token is invalid");
            }
            var user = await _userRepository.FindUserAsync(refreshTokenEntity.UserId, ct);

            if (user is null)
            {
                throw new UnauthorizedAccessException("User cannot found with specified token");
            }

            var jwt = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var hashedNewRefreshToken = _tokenService.HashToken(newRefreshToken);

            var newRefreshTokenEntity = new ReactASP.Domain.Entities.RefreshToken
            {
                UserId = user.UserId,
                TokenHash = hashedNewRefreshToken
            };

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, ct);
            await _userRepository.SaveChangesAsync(ct);

            return Result.Success(new RefreshTokenResult(jwt, newRefreshToken));
        }
    }
}
