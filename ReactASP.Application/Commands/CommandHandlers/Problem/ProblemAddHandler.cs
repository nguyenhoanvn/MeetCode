using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Interfaces;
using Microsoft.Extensions.Logging;
using ReactASP.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;

namespace ReactASP.Application.Commands.CommandHandlers.Problem
{
    public sealed class ProblemAddHandler : IRequestHandler<ProblemAddCommand, Result<ProblemAddResult>>
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
        public async Task<Result<ProblemAddResult>> Handle(ProblemAddCommand request, CancellationToken ct)
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
                ct);

            _logger.LogInformation("New problem added successfully");
            return Result.Success(new ProblemAddResult(problem.ProblemId,
                problem.Slug,
                problem.Title,
                problem.StatementMd,
                problem.Difficulty,
                problem.TimeLimitMs,
                problem.MemoryLimitMb,
                problem.CreatedAt,
                problem.CreatedBy));
        }
    }
}
