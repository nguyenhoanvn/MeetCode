using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
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
    public class ProblemTemplateReadQueryHandler : IRequestHandler<ProblemTemplateReadQuery, Result<ProblemTemplateReadQueryResult>>
    {
        private readonly IProblemTemplateService _problemTemplateService;
        private readonly IProblemService _problemService;
        private readonly ILanguageService _languageService;
        private readonly ILogger<ProblemTemplateReadQueryHandler> _logger;
        public ProblemTemplateReadQueryHandler(
            IProblemTemplateService problemTemplateService,
            IProblemService problemService,
            ILanguageService languageService,
            ILogger<ProblemTemplateReadQueryHandler> logger)
        {
            _problemTemplateService = problemTemplateService;
            _problemService = problemService;
            _logger = logger;
        }

        public async Task<Result<ProblemTemplateReadQueryResult>> Handle(ProblemTemplateReadQuery request, CancellationToken ct)
        {
            var problemTemplate = await _problemTemplateService.FindTemplateBySlugAsync(request.ProblemSlug, ct);
            if (problemTemplate == null)
            {
                _logger.LogWarning($"Problem template with slug {request.ProblemSlug} not found");
                return Result.NotFound($"Problem template with slug {request.ProblemSlug} not found");
            }

            var result = new ProblemTemplateReadQueryResult(problemTemplate, problemTemplate.Languages, problemTemplate.Problems);
            return result;
        }
    }
}
