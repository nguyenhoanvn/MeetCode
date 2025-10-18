using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Problem;
using MeetCode.Application.Queries.QueryResults.Problem;
using Microsoft.Extensions.Logging;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandHandlers.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;

namespace MeetCode.Application.Queries.QueryHandlers.Problem
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
