using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.Problem;
using MeetCode.Application.Queries.QueryResults.Problem;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Problem
{
    public class ProblemReadByIdQueryHandler : IRequestHandler<ProblemReadByIdQuery, Result<ProblemReadQueryResult>>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly ILogger<ProblemReadByIdQueryHandler> _logger;
        public ProblemReadByIdQueryHandler(
            IProblemRepository problemRepository,
            ILogger<ProblemReadByIdQueryHandler> logger
            )
        {
            _problemRepository = problemRepository;
            _logger = logger;
        }

        public async Task<Result<ProblemReadQueryResult>> Handle(ProblemReadByIdQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to find problem with Id {ProblemId}", request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(request.ProblemId, ct);

            if (problem == null)
            {
                _logger.LogWarning("Cannot find problem with Id {ProblemId}", request.ProblemId);
                return Result.Invalid(new ValidationError(nameof(request.ProblemId), $"Cannot find problem with Id {request.ProblemId}"));
            }

            _logger.LogInformation("Problem with Id {ProblemId} found {Problem}", request.ProblemId, problem.ToGenericString());
            return Result.Success(new ProblemReadQueryResult(problem));
        }
    }
}
