using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(Guid userId, string email, string role);
        string GenerateRefreshToken();
        string HashToken(string unhashedToken);
        Task<RefreshToken?> FindRefreshTokenByTokenAsync(string plainRefreshToken, CancellationToken ct);
        Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, string plainRefreshToken, CancellationToken ct);
        Task<RefreshToken?> FindRefreshTokenAsync(Guid refreshTokenId, CancellationToken ct);
        Task<RefreshToken?> FindRefreshTokenByUserIdAsync(Guid refreshTokenId, CancellationToken ct);
        Task<bool> InvalidateRefreshToken(Guid userId, CancellationToken ct);
    }
}
