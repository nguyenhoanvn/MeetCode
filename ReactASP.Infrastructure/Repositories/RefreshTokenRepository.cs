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
        public async Task<RefreshToken> GetById(Guid refreshTokenId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            await _db.RefreshTokens.AddAsync(refreshToken, ct);
        }
        public async Task UpdateAsync(Guid refreshTokenId, RefreshToken refreshToken, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public async Task DeletedAsync(Guid refreshTokenId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<RefreshToken?> GetByToken(string hashedToken, CancellationToken ct)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == hashedToken, ct);
        }

    }
}
