using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Profile;
using MeetCode.Application.Queries.QueryResults.Profile;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Profile
{
    public class ProfileUserQueryHandler : IRequestHandler<ProfileUserQuery, Result<ProfileUserQueryResult>>
    {
        private readonly ILogger<ProfileUserQueryHandler> _logger;
        private readonly IProfileService _profileService;
        
        public ProfileUserQueryHandler(
            ILogger<ProfileUserQueryHandler> logger,
            IProfileService profileService)
        {
            _logger = logger;
            _profileService = profileService;
        }

        public async Task<Result<ProfileUserQueryResult>> Handle(ProfileUserQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to get profile of user {request.UserId}");

            var user = await _profileService.GetCurrentUser(request.UserId, ct);

            if (user == null)
            {
                _logger.LogWarning($"User with id {request.UserId} not found");
                return Result.NotFound($"User with id {request.UserId} not found");
            }

            var result = new ProfileUserQueryResult(user.DisplayName);

            return result;
        }
    }
}
