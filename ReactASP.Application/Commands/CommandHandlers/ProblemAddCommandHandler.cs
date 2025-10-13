using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Common;
using Microsoft.Extensions.Logging;
using ReactASP.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Application.Commands.CommandEntities;
using ReactASP.Application.Commands.CommandResults;

namespace ReactASP.Application.Commands.ProblemAdd
{
    public sealed class ProblemAddCommandHandler : IRequestHandler<ProblemAddCommand, Result<ProblemAddResult>>
    {
        private readonly ILogger<ProblemAddCommandHandler> _logger;
        private readonly ISessionService _sessionService;
        private readonly IProblemService _problemService;
        public ProblemAddCommandHandler(
            ISessionService sessionService,
            IProblemService problemService,
            ILogger<ProblemAddCommandHandler> logger)
        {
            _sessionService = sessionService;
            _problemService = problemService;
            _logger = logger;
        }
        public async Task<Result<ProblemAddResult>> Handle(ProblemAddCommand request, CancellationToken ct)
        {
            try
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
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return Result.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected exception occured: {ex.Message}");
                return Result.Error("An unexpected exception occured while trying to create new problem");
            }
        }
    }
}
