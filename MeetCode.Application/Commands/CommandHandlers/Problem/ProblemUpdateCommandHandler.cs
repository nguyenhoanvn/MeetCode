using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.Problem
{
    public class ProblemUpdateCommandHandler : IRequestHandler<ProblemUpdateCommand, Result<ProblemUpdateCommandResult>>
    {
        private readonly ILogger<ProblemUpdateCommandHandler> _logger;
        private readonly IProblemService _problemService;
        public ProblemUpdateCommandHandler(
            ILogger<ProblemUpdateCommandHandler> logger,
            IProblemService problemService
            )
        {
            _logger = logger;
            _problemService = problemService;
        }
        public async Task<Result<ProblemUpdateCommandResult>> Handle(ProblemUpdateCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to update problem with slug {request.Slug}");
            var problem = await _problemService.FindProblemBySlugAsync(request.Slug, ct);
            
            if (problem == null)
            {
                _logger.LogWarning($"Problem with slug {request.Slug} cannot be found");
                throw new InvalidOperationException($"Problem with slug {request.Slug} cannot be found");
            }

            problem.Title = request.NewTitle;
            problem.Difficulty = request.NewDifficulty;
            problem.StatementMd = request.NewStatementMd;
            problem.GenerateSlug();
            var updatedProblem = await _problemService.UpdateProblemAsync(problem, ct);
            if (updatedProblem == null)
            {
                _logger.LogWarning($"Updated problem cannot be found");
                throw new InvalidOperationException($"Updated problem cannot be found");
            }

            var result = new ProblemUpdateCommandResult(updatedProblem);
            return Result.Success(result);

        }
    }
}
