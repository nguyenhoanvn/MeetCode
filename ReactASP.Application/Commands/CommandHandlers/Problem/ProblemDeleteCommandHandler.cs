using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Interfaces.Services;

namespace ReactASP.Application.Commands.CommandHandlers.Problem
{
    public class ProblemDeleteCommandHandler : IRequestHandler<ProblemDeleteCommand, Result<ProblemDeleteCommandResult>>
    {
        private readonly ILogger<ProblemDeleteCommandHandler> _logger;
        private readonly IProblemService _problemService;
        public ProblemDeleteCommandHandler(
            ILogger<ProblemDeleteCommandHandler> logger,
            IProblemService problemService)
        {
            _logger = logger;
            _problemService = problemService;
        }

        public async Task<Result<ProblemDeleteCommandResult>> Handle(ProblemDeleteCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to delete problem with slug: {request.Slug}");

            var problem = await _problemService.FindProblemBySlugAsync(request.Slug, ct);
            if (problem == null)
            {
                _logger.LogWarning($"Delete failed because cannot found problem with slug {request.Slug}");
                throw new InvalidOperationException($"Delete failed because cannot found problem with slug {request.Slug}");
            }
            await _problemService.DeleteProblemAsync(problem, ct);
            _logger.LogInformation($"Delete problem with slug {request.Slug} successfully");
            var result = new ProblemDeleteCommandResult($"Delete problem with slug {request.Slug} successfully");
            return Result.Success(result);
        }
    }
}
