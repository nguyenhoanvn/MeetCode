using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Auth
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<LogoutCommandResult>>
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<LogoutCommandHandler> _logger;
        public LogoutCommandHandler(
            ITokenService tokenService,
            ILogger<LogoutCommandHandler> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }
        public async Task<Result<LogoutCommandResult>> Handle(LogoutCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"User {request.UserId} logout started");

            if (!(await _tokenService.InvalidateRefreshToken(request.UserId, ct)))
            {
                return Result.Error("Something went wrong when trying to invalidate refresh token");
            }

            return Result.Success(new LogoutCommandResult(true));
        }
    }
}
