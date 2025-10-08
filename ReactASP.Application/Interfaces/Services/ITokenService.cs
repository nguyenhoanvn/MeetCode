using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(Guid userId, string email, string role);
        string GenerateRefreshToken();
        string HashToken(string unhashedToken);
        Task<RefreshToken> FindRefreshTokenAsync(string hashedToken, CancellationToken ct);
        Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, string plainRefreshToken, CancellationToken ct);
    }
}
