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
using MeetCode.Application.Commands.CommandHandlers.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;

namespace MeetCode.Application.Queries.QueryHandlers.Problem
{
    public class ProblemReadQueryHandler : IRequestHandler<ProblemReadQuery, Result<ProblemReadQueryResult>>
    {
        private readonly ILogger<ProblemReadQueryHandler> _logger;
        private readonly IProblemService _problemService;
        public ProblemReadQueryHandler(
            ILogger<ProblemReadQueryHandler> logger,
            IProblemService problemService
            )
        {
            _logger = logger;
            _problemService = problemService;
        }
        public async Task<Result<ProblemReadQueryResult>> Handle(ProblemReadQuery request, CancellationToken ct)
        {
            var problem = await _problemService.FindProblemBySlugAsync(request.ProblemSlug, ct);
            if (problem == null)
            {
                _logger.LogWarning("Read problem failed because cannot find the specified problem");
                throw new InvalidOperationException("Read problem failed because cannot find the specified problem");
            }
            var result = new ProblemReadQueryResult(problem);
            return Result.Success(result);
        }
    }
}
