using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MeetCode.Domain.Entities;

namespace MeetCode.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SessionService> _logger;
        public SessionService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<SessionService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public Result<Guid> ExtractUserIdFromJwt(CancellationToken ct)
        {
            _logger.LogInformation($"Extracting user Id from JWT (access token)");

            var userClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                _logger.LogWarning("Extract JWT Sub is failed, Sub is invalid");
                return Result.Unauthorized("You are not logged in");
            }

            if (!Guid.TryParse(userClaim.Value, out var userId))
            {
                _logger.LogWarning("Cannot parse result to user Id");
                return Result.Error(new ErrorList(new[] { "Internal error occured" }));
            }

            _logger.LogInformation("Extracting user Id successfully");
            return Result.Success(userId);
        }
    }
}
