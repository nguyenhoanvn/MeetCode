using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReactASP.Application.Interfaces;

namespace ReactASP.Server.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public TokenService(
            IConfiguration config,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _config = config;
            _refreshTokenRepository = refreshTokenRepository;
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
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

        public string HashToken(string unhashedToken) { 
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(unhashedToken));
                return Convert.ToBase64String(bytes);
            }
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken, CancellationToken ct)
        {
            /*var hashedRefreshToken = HashToken(refreshToken);*/
            var hashedRefreshToken = refreshToken;
            var token = await _refreshTokenRepository.GetByToken(hashedRefreshToken, ct);

            if (token is null)
            {
                throw new ArgumentException("No refresh token is found");
            }

            if (token.ExpiresAt < DateTime.UtcNow || token.IsRevoked) return false;

            return token.TokenHash == hashedRefreshToken;
        }
    }
}
