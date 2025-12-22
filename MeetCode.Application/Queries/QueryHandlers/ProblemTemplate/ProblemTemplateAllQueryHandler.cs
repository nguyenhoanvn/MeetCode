using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.ProblemTemplate;
using MeetCode.Application.Queries.QueryResults.ProblemTemplate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.ProblemTemplate
{
    public class ProblemTemplateAllQueryHandler : IRequestHandler<ProblemTemplateAllQuery, Result<ProblemTemplateAllQueryResult>>
    {
        private readonly IProblemTemplateRepository _problemTemplateRepository;
        private readonly ILogger<ProblemTemplateAllQueryHandler> _logger;
        public ProblemTemplateAllQueryHandler(
            IProblemTemplateRepository problemTemplateRepository,
            ILogger<ProblemTemplateAllQueryHandler> logger
            )
        {
            _problemTemplateRepository = problemTemplateRepository;
            _logger = logger;
        }
        public async Task<Result<ProblemTemplateAllQueryResult>> Handle(ProblemTemplateAllQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to retrieve problem template list");

            var templateList = (await _problemTemplateRepository.GetAsync(ct)).ToList();

            _logger.LogInformation("Retrieved problem template list with size: {Size}", templateList.Count());

            return Result.Success(new ProblemTemplateAllQueryResult(templateList));
        }
    }
}
