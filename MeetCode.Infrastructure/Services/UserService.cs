using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ardalis.Result;
using Azure.Core;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Enums;
using MeetCode.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MeetCode.Infrastructure.Services
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
        public async Task<User?> FindUserByEmailAsync(string email, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to find user by email");
            var user = await _userRepository.GetUserByEmailWithTokensAsync(email, ct);
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

        public async Task<User?> FindUserAsync(Guid userId, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", userId);
            var user = await _userRepository.GetByIdAsync(userId, ct);
            _logger.LogInformation("User {UserId} found successfully", userId);
            return user;
        }

        public string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }
        public async Task<User?> CreateUserAsync(string email, string displayName, string plainPassword, CancellationToken ct)
        {
            var existingUser = await FindUserByEmailAsync(email, ct);

            if (existingUser != null)
            {
                _logger.LogWarning($"User with email {email} already exists");
                throw new DuplicateEntityException<User>(new Dictionary<string, string>
                {
                    { nameof(existingUser.Email), existingUser.Email }
                });
            }

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                DisplayName = displayName,
                Role = UserRole.User,
                PasswordHash = HashPassword(plainPassword),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddAsync(user, ct);

            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                _logger.LogWarning($"Failed to add user {email} to the database");
                throw new DbUpdateException("Failed to save the new problem to the database.");
            }
            return user;
        }

    }
}
