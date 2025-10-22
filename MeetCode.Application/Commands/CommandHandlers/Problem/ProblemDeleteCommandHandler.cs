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
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.Problem
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
            _logger.LogInformation($"Attempting to delete problem with slug: {request.ProblemId}");

            var problem = await _problemService.FindProblemByIdAsync(request.ProblemId, ct);
            if (problem == null)
            {
                _logger.LogWarning($"Delete failed because cannot found problem with Id {request.ProblemId}");
                return Result.NotFound($"Delete failed because cannot found problem with Id {request.ProblemId}");
            }
            await _problemService.DeleteProblemAsync(problem, ct);
            _logger.LogInformation($"Delete problem with Id {request.ProblemId} successfully");
            var result = new ProblemDeleteCommandResult($"Delete problem with Id {request.ProblemId} successfully");
            return Result.Success(result);
        }
    }
}
