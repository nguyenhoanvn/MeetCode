using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Commands.CommandEntities.Problem;
using ReactASP.Application.Commands.CommandHandlers.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Application.Queries.QueryEntities.Problem;
using ReactASP.Application.Queries.QueryResults.Problem;

namespace ReactASP.Application.Queries.QueryHandlers.Problem
{
    public class ProblemAllQueryHandler : IRequestHandler<ProblemAllQuery, Result<ProblemAllQueryResult>>
    {
        private readonly ILogger<ProblemAllQueryHandler> _logger;
        private readonly IProblemService _problemService;
        public ProblemAllQueryHandler(
            ILogger<ProblemAllQueryHandler> logger,
            IProblemService problemService)
        {
            _logger = logger;
            _problemService = problemService;
        }
        public async Task<Result<ProblemAllQueryResult>> Handle(ProblemAllQuery request, CancellationToken ct)
        {
            var problemList = await _problemService.ReadAllProblemsAsync(ct);
            if (problemList == null)
            {
                throw new InvalidOperationException("Problem list is null");
            }
            var result = new ProblemAllQueryResult(problemList);
            return Result.Success(result);
        }
    }
}
