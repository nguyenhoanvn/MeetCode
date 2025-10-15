using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ardalis.Result;
using Azure.Core;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;

namespace ReactASP.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<User> FindUserByEmailAsync(string email, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to find user by email");
            var user = await _userRepository.GetUserByEmailWithTokensAsync(email, ct);

            if (user == null)
            {
                _logger.LogWarning("User not found for provided email");
                throw new InvalidOperationException($"User with email: {email} not found");
            }
            _logger.LogInformation("User found for provided email");
            return user;
        }

        public bool IsPasswordMatch(string requestPassword, string userDbPassword)
        {
            if (requestPassword == null || userDbPassword == null)
            {
                _logger.LogError("Password input or stored hash is null");
                throw new InvalidOperationException("Password input is null");
            }
            var isMatch = BCrypt.Net.BCrypt.Verify(requestPassword, userDbPassword);
            _logger.LogInformation("Password verification result: {Result}", isMatch);
            return isMatch;  
        }

        public async Task<User> FindUserAsync(Guid userId, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", userId);
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID {UserId}", userId);
                throw new InvalidOperationException($"User cannot be found with id: {userId}");
            }
            _logger.LogInformation("User {UserId} found successfully", userId);
            return user;
        }

        
    }
}
