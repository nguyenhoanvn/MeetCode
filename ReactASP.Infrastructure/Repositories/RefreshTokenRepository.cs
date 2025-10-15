using Microsoft.EntityFrameworkCore;
using ReactASP.Application.Interfaces;
using ReactASP.Domain.Entities;
using ReactASP.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;

        public RefreshTokenRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RefreshToken>> GetAsync(CancellationToken ct)
        {
            return await _db.RefreshTokens.ToListAsync(ct);
        }

        public async Task<RefreshToken?> GetByIdAsync(Guid refreshTokenId, CancellationToken ct)
        {
            return await _db.RefreshTokens.FindAsync(refreshTokenId, ct);
        }
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            await _db.RefreshTokens.AddAsync(refreshToken, ct);
        }
        public Task Update(RefreshToken refreshToken, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteAsync(Guid refreshTokenId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<RefreshToken?> GetByToken(string hashedToken, CancellationToken ct)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == hashedToken, ct);
        }

        public async Task<RefreshToken?> GetByUserId(Guid userId, CancellationToken ct)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId, ct);
        }
    }
}
