using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJwtToken(Guid userId, string email, string role);
        public string GenerateRefreshToken();
        public string HashToken(string unhashedToken);
        public Task<bool> ValidateRefreshToken(string refreshToken, CancellationToken ct);
    }
}
