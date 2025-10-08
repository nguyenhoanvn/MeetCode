using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Azure.Core;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;

namespace ReactASP.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> FindUserByEmailAsync(string email, CancellationToken ct)
        {
            var user = await _userRepository.GetUserByEmailWithTokensAsync(email, ct);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            return user;
        }

        public bool IsPasswordMatch(string requestPassword, string userDbPassword)
        {
            if (requestPassword == null || userDbPassword == null)
            {
                throw new InvalidOperationException("Password input is null");
            }
            
            return BCrypt.Net.BCrypt.Verify(requestPassword, userDbPassword);  
        }

        public async Task<User> FindUserAsync(Guid userId, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user == null)
            {
                throw new InvalidOperationException($"User cannot be found with id: {userId}");
            }
            return user;
        }
    }
}
