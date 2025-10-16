using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Commands.CommandHandlers.Problem;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Application.Queries.QueryEntities.Problem;
using ReactASP.Application.Queries.QueryResults.Problem;

namespace ReactASP.Application.Queries.QueryHandlers.Problem
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
