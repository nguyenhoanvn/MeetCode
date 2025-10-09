using ReactASP.Application.Interfaces.Repositories;
using ReactASP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByToken(string hashedToken, CancellationToken ct);
        Task<RefreshToken?> GetByUserId(Guid userId, CancellationToken ct);
    }
}
