using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MeetCode.Infrastructure.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TokenService> _logger;
        public TokenService(
            IConfiguration config,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<TokenService> logger)
        {
            _config = config;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public string GenerateJwtToken(Guid userId, string email, string role)
        {
            _logger.LogInformation("Generating new JTW token started");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            _logger.LogInformation("New JWT token issued successfully");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string HashToken(string unhashedToken)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(unhashedToken));
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<RefreshToken?> FindRefreshTokenByTokenAsync(string plainRefreshToken, CancellationToken ct)
        {
            _logger.LogInformation("Find Refresh token based on token started");
            var hashedToken = HashToken(plainRefreshToken);
            var refreshTokenEntity = await _refreshTokenRepository.GetByToken(hashedToken, ct);

            if (refreshTokenEntity == null)
            {
                _logger.LogWarning("Refresh token with input token not found");
                return null;
            }
            if (!refreshTokenEntity.IsValid())
            {
                _logger.LogWarning("Refresh token is revoked or expired");
                return null;
            }
            _logger.LogInformation("Refresh token found successfully");
            return refreshTokenEntity;
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, string plainRefreshToken, CancellationToken ct)
        {
            _logger.LogInformation("Creating refresh token started");
            await _unitOfWork.BeginTransactionAsync(ct);

            // Revoke old tokens
            var userWithTokens = await _userRepository.GetUserByIdWithTokensAsync(userId, ct);
            if (userWithTokens == null)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                _logger.LogWarning($"Cannot find the user {userId}");
                throw new InvalidOperationException($"User not found with id: {userId}");
            }

            var oldTokenList = userWithTokens.RefreshTokens
                .Where(rt => !rt.IsRevoked && rt.ExpiresAt > DateTimeOffset.UtcNow)
                .ToList();
            if (oldTokenList.Count() > 0)
            {
                foreach (var oldToken in oldTokenList)
                {
                    oldToken.IsRevoked = true;
                }
            }
            
            // Add new token
            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                TokenHash = HashToken(plainRefreshToken),
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken, ct);

            // Check saved
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                _logger.LogWarning("Cannot saved changes to the refresh token");
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw new InvalidOperationException("Failed to save new refresh token");
            }

            await _unitOfWork.CommitTransactionAsync(ct);
            return newRefreshToken;
        }

        public async Task<RefreshToken?> FindRefreshTokenByUserIdAsync(Guid userId, CancellationToken ct)
        {
            var refreshTokenEntity = await _refreshTokenRepository.GetByUserId(userId, ct);
            if (refreshTokenEntity == null)
            {
                _logger.LogWarning($"Refresh token of user {userId} not found");
                return null;
            }
            if (!refreshTokenEntity.IsValid())
            {
                _logger.LogWarning($"Refresh token is revoked or expired");
                return null;
            }
            return refreshTokenEntity;
        }

        public async Task<RefreshToken?> FindRefreshTokenAsync(Guid refreshTokenId, CancellationToken ct)
        {
            var refreshTokenEntity = await _refreshTokenRepository.GetByIdAsync(refreshTokenId, ct);
            if (refreshTokenEntity == null)
            {
                _logger.LogWarning("Refresh token with inputted id not found");
                return null;
            }
            if (!refreshTokenEntity.IsValid())
            {
                _logger.LogWarning("Refresh token is revoked or expired");
                return null;
            }
            return refreshTokenEntity;
        }
        public async Task<bool> InvalidateRefreshToken(Guid userId, CancellationToken ct)
        {
            var refreshTokenEntity = await _refreshTokenRepository.GetByUserId(userId, ct);
            if (refreshTokenEntity == null)
            {
                _logger.LogWarning($"Refresh token with user id {userId} not found");
                return false;
            }
            if (!refreshTokenEntity.IsValid())
            {
                _logger.LogWarning("Refresh token is revoked or expired");
                return false;
            }
            refreshTokenEntity.IsRevoked = true;
            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation($"Refresh token with userId {userId} revoked");
            return true;
        }
    }
}
