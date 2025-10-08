using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ReactASP.Application.Commands.RefreshToken;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;

namespace ReactASP.Server.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly int RT_EXPIRE_DAY = 30;

        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TokenService(
            IConfiguration config,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _config = config;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public string GenerateJwtToken(Guid userId, string email, string role)
        {
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
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: creds);

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

        public async Task<RefreshToken> FindRefreshTokenAsync(string hashedToken, CancellationToken ct)
        {
            var refreshTokenEntity = await _refreshTokenRepository.GetByToken(hashedToken, ct);

            if (refreshTokenEntity == null)
            {
                throw new InvalidOperationException("Refresh token with the hashed token not found");
            }
            if (!refreshTokenEntity.IsValid())
            {
                throw new InvalidOperationException("Refresh token expired");
            }
            return refreshTokenEntity;
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, string plainRefreshToken, CancellationToken ct)
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            // Revoke old tokens
            var userWithTokens = await _userRepository.GetUserByIdWithTokensAsync(userId, ct);
            if (userWithTokens == null)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw new InvalidOperationException($"User not found with id: {userId}");
            }

            var oldTokenList = userWithTokens.RefreshTokens
                .Where(rt => !rt.IsRevoked && rt.ExpiresAt > DateTimeOffset.UtcNow)
                .ToList();

            foreach (var oldToken in oldTokenList)
            {
                oldToken.IsRevoked = true;
            }

            // Add new token
            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                TokenHash = HashToken(plainRefreshToken),
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(RT_EXPIRE_DAY)
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken, ct);

            // Check saved
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw new InvalidOperationException("Failed to save new refresh token");
            }

            await _unitOfWork.CommitTransactionAsync(ct);
            return newRefreshToken;
        }
    }
}
