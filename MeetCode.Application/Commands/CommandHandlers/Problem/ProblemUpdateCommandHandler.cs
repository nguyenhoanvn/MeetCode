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
            _logger.LogInformation($"Attempting to update problem with id {request.ProblemId}");  

            var updatedProblem = await _problemService.UpdateProblemAsync(
                request.ProblemId, 
                request.NewTitle, 
                request.NewStatementMd,
                request.NewDifficulty, 
                request.TagIds, 
                ct);

            var result = new ProblemUpdateCommandResult(updatedProblem);
            return Result.Success(result);

        }
    }
}
