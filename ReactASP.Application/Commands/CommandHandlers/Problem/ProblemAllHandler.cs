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
    public class ProblemAllHandler : IRequestHandler<ProblemAllCommand, Result<ProblemAllResult>>
    {
        private readonly ILogger<ProblemAllHandler> _logger;
        private readonly IProblemService _problemService;
        public ProblemAllHandler(
            ILogger<ProblemAllHandler> logger,
            IProblemService problemService)
        {
            _logger = logger;
            _problemService = problemService;
        }
        public async Task<Result<ProblemAllResult>> Handle(ProblemAllCommand request, CancellationToken ct)
        {
            var problemList = await _problemService.ReadAllProblemsAsync(ct);
            if (problemList == null)
            {
                throw new InvalidOperationException("Problem list is null");
            }
            var result = new ProblemAllResult(problemList);
            return Result.Success(result);
        }
    }
}
