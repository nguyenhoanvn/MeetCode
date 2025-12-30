using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.Profile;
using MeetCode.Application.Queries.QueryResults;
using MeetCode.Domain.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Profile
{
    public class CurrentUserQueryHandler : IRequestHandler<CurrentUserQuery, Result<CurrentUserQueryResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CurrentUserQueryHandler> _logger;
        public CurrentUserQueryHandler(
            IUserRepository userRepository,
            ILogger<CurrentUserQueryHandler> logger
            )
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result<CurrentUserQueryResult>> Handle(CurrentUserQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to get current admin user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, ct);

            if (user == null)
                return Result.Unauthorized();

            _logger.LogInformation("{AdminOrUser} found with Id {UserId}", user.Role, user.UserId);

            return Result.Success(new CurrentUserQueryResult(user));
        }
    }
}
