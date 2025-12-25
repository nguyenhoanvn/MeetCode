using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using MeetCode.Application.Commands.CommandResults.ProblemTemplate;
using MeetCode.Application.Interfaces.Helpers;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.ProblemTemplate
{
    public class ProblemTemplateAddCommandHandler : IRequestHandler<ProblemTemplateAddCommand, Result<ProblemTemplateAddCommandResult>>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IProblemTemplateRepository _problemTemplateRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemTemplateAddCommandHandler> _logger;

        private readonly Dictionary<string, ILanguageTemplateGenerator> _templateMap;

        public ProblemTemplateAddCommandHandler(
            IProblemRepository problemRepository,
            IProblemTemplateRepository problemTemplateRepository,
            ILanguageRepository languageRepository,
            IUnitOfWork unitOfWork,
            IEnumerable<ILanguageTemplateGenerator> generators,
            ILogger<ProblemTemplateAddCommandHandler> logger)
        {
            _problemRepository = problemRepository;
            _problemTemplateRepository = problemTemplateRepository;
            _languageRepository = languageRepository;
            _unitOfWork = unitOfWork;
            _templateMap = generators.ToDictionary(g => g.LanguageName.ToLower());
            _logger = logger;
        }
        public async Task<Result<ProblemTemplateAddCommandResult>> Handle(ProblemTemplateAddCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to add new problem template {Request}", request.ToGenericString());

            // Check existence
            if ((await _problemRepository.GetByIdAsync(request.ProblemId, ct)) == null)
            {
                _logger.LogWarning("Cannot find problem {ProblemId}", request.ProblemId);
                return Result.Invalid(new ValidationError(nameof(request.ProblemId), $"Cannot find problem {request.ProblemId}"));
            }

            var language = await _languageRepository.GetByIdAsync(request.LangId, ct);
            if (language == null)
            {
                _logger.LogWarning("Cannot find language {LangId}", request.LangId);
                return Result.Invalid(new ValidationError(nameof(request.LangId), $"Cannot find language {request.LangId}"));
            }

            if (await _problemTemplateRepository.IsProblemTemplateExistsAsync(request.ProblemId, request.LangId, ct))
            {
                _logger.LogWarning("Template associated with problem {ProblemId} and language {LangId} exists", request.ProblemId, request.LangId);
                return Result.Invalid(new ValidationError($"{nameof(request.ProblemId)}, {nameof(request.LangId)}", $"Template associated with problem {request.ProblemId} and language {request.LangId} exists"));
            }

            if (!_templateMap.TryGetValue(language.Name.ToLower(), out var generator))
            {
                _logger.LogWarning("Template generator for {LanguageName} not supported", language.Name);
                return Result.Invalid(new ValidationError(nameof(language.Name), $"Template generator for language {language.Name} not supported"));
            }

            var templateCode = generator.GenerateTemplate(request.MethodName, request.ReturnType, request.Parameters);
            var runnerCode = generator.GenerateRunner(request.MethodName, request.Parameters);

            var problemTemplate = new MeetCode.Domain.Entities.ProblemTemplate
            {
                ProblemId = request.ProblemId,
                LangId = request.LangId,
                TemplateCode = templateCode,
                RunnerCode = runnerCode,
                CompileCommand = request.CompileCommand,
                RunCommand = request.RunCommand
            };

            await _problemTemplateRepository.AddAsync(problemTemplate, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Created(new ProblemTemplateAddCommandResult(problemTemplate));
        }
    }
}
