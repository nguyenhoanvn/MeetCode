using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces;
using Microsoft.Extensions.Logging;
using MeetCode.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Commands.CommandEntities.Problem;

namespace MeetCode.Application.Commands.CommandHandlers.Problem
{
    public sealed class ProblemAddHandler : IRequestHandler<ProblemAddCommand, Result<ProblemAddCommandResult>>
    {
        private readonly ILogger<ProblemAddHandler> _logger;
        private readonly ISessionService _sessionService;
        private readonly IProblemService _problemService;
        public ProblemAddHandler(
            ISessionService sessionService,
            IProblemService problemService,
            ILogger<ProblemAddHandler> logger)
        {
            _sessionService = sessionService;
            _problemService = problemService;
            _logger = logger;
        }
        public async Task<Result<ProblemAddCommandResult>> Handle(ProblemAddCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Problem add handler started for {request.Title}");
            var userId = _sessionService.ExtractUserIdFromJwt(ct);

            var problem = await _problemService.CreateProblemAsync(
                request.Title,
                request.StatementMd,
                request.Difficulty,
                request.TimeLimitMs,
                request.MemoryLimitMb,
                userId,
                request.TagIds,
                ct);

            _logger.LogInformation("New problem added successfully");
            var result = new ProblemAddCommandResult(problem);
            return Result.Success(result);
        }
    }
}
