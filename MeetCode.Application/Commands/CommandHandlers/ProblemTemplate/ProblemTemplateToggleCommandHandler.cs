using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using MeetCode.Application.Commands.CommandResults.ProblemTemplate;
using MeetCode.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.ProblemTemplate
{
    public class ProblemTemplateToggleCommandHandler : IRequestHandler<ProblemTemplateToggleCommand, Result<ProblemTemplateToggleCommandResult>>
    {
        private readonly IProblemTemplateRepository _problemTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemTemplateToggleCommandHandler> _logger;
        public ProblemTemplateToggleCommandHandler(
            IProblemTemplateRepository problemTemplateRepository,
            IUnitOfWork unitOfWork,
            ILogger<ProblemTemplateToggleCommandHandler> logger
            )
        {
            _problemTemplateRepository = problemTemplateRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProblemTemplateToggleCommandResult>> Handle(ProblemTemplateToggleCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to toggling status of template {TemplateId}", request.TemplateId);

            var template = await _problemTemplateRepository.GetByIdAsync(request.TemplateId, ct);

            if (template == null)
            {
                _logger.LogWarning("Cannot find template with Id {TemplateId}", request.TemplateId);
                return Result.Invalid(new ValidationError(nameof(request.TemplateId), $"Cannot find template with Id {request.TemplateId}"));
            }

            template.ToggleStatus();

            await _problemTemplateRepository.Update(template);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Problem template {Template} to {Status}", template, template.IsEnabled);
            return Result.Success(new ProblemTemplateToggleCommandResult(template));
        }
    }
}
