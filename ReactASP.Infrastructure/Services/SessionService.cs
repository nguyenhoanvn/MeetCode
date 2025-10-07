using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReactASP.Application.Interfaces;
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
        public Claim? GetUserClaim(CancellationToken ct)
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);  
        }
    }
}
