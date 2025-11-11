using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public AuthService(
            IUserRepository userRepository,
            ILogger<AuthService> logger,
            IUnitOfWork unitOfWork,
            ICacheService cacheService
            )
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        public async Task UpdateLoginTime(Guid userId, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to update login time for user with ID {UserId}", userId);
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID {UserId}", userId);
                throw new InvalidOperationException($"User cannot be found with id: {userId}");
            }
            user.LastLoginAt = DateTimeOffset.UtcNow;
            await _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation("Successfully updated login time for user {UserId}", userId);
        }

        public async Task<string> GetEmailFromOtpAsync(string otp)
        {
            _logger.LogInformation("Attempting to look for email in cache");
            var email = await _cacheService.GetValueAsync<string>($"auth:resetpassword:otp:{otp}");
            if (email == null)
            {
                _logger.LogWarning("Email not found in cache");
                throw new InvalidOperationException("Incorrect Otp");
            }
            return email;
        }
    }
}
