using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Problem;
using MeetCode.Application.Queries.QueryResults.Problem;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Problem
{
    public class ProblemSearchQueryHandler : IRequestHandler<ProblemSearchQuery, Result<ProblemAllQueryResult>>
    {
        private readonly IProblemService _problemService;
        private readonly ILogger<ProblemSearchQueryHandler> _logger;
        public ProblemSearchQueryHandler(
            IProblemService problemService,
            ILogger<ProblemSearchQueryHandler> logger)
        {
            _problemService = problemService;
            _logger = logger;
        }

        public async Task<Result<ProblemAllQueryResult>> Handle(ProblemSearchQuery request, CancellationToken ct)
        {
            string slug = Regex.Replace(request.ProblemName.ToLowerInvariant().Trim(), @"\s+", "-").ToLowerInvariant();

            var problemList = await _problemService.ReadAllProblemsBySlugAsync(slug, ct);

            if (problemList.Count() == 0)
            {
                _logger.LogInformation($"The problem with name {request.ProblemName} does not exists");
            }

            var result = new ProblemAllQueryResult(problemList);
            return Result.Success(result);

        }
    }
}
