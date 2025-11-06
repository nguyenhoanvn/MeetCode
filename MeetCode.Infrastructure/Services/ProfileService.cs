using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IUserRepository _userRepository;

        public ProfileService(
            ILogger<ProfileService> logger,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<User?> GetCurrentUser(Guid userId, CancellationToken ct)
        {
            return await _userRepository.GetByIdAsync(userId, ct);
        } 
    }
}
