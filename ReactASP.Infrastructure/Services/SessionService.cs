using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;

namespace ReactASP.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionService(IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid ExtractUserIdFromJwt(CancellationToken ct)
        {
            var userClaim = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userClaim == null)
            {
                throw new InvalidOperationException("JWT does not contain a 'sub' (UserId) claim");
            }
            if (!Guid.TryParse(userClaim.Value, out var userId))
            {
                throw new InvalidOperationException($"Invalid userId format in JWT claim: {userClaim.Value}")
            }
            return userId;
        }
    }
}
