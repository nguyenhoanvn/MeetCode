using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.ProblemTemplate;
using MeetCode.Application.Queries.QueryResults.ProblemTemplate;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.ProblemTemplate
{
    public class ProblemTemplateReadByIdQueryHandler : IRequestHandler<ProblemTemplateReadByIdQuery, Result<ProblemTemplateReadQueryResult>>
    {
        private readonly IProblemTemplateRepository _problemTemplateRepository;
        private readonly ILogger<ProblemTemplateReadByIdQueryHandler> _logger;
        public ProblemTemplateReadByIdQueryHandler(
            IProblemTemplateRepository problemTemplateRepository,
            ILogger<ProblemTemplateReadByIdQueryHandler> logger)
        {
            _problemTemplateRepository = problemTemplateRepository;
            _logger = logger;
        }
        public async Task<Result<ProblemTemplateReadQueryResult>> Handle(ProblemTemplateReadByIdQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to read template {TemplateId}", request.TemplateId);

            var template = await _problemTemplateRepository.GetByIdAsync(request.TemplateId, ct);

            if (template == null)
            {
                _logger.LogWarning("Cannot find template {TemplateId}", request.TemplateId);
                return Result.Invalid(new ValidationError(nameof(request.TemplateId), $"Cannot find template {request.TemplateId}."));
            }

            _logger.LogInformation("Problem template found {Template}", template.ToGenericString());

            return Result.Success(new ProblemTemplateReadQueryResult(template));
        }
    }
}
