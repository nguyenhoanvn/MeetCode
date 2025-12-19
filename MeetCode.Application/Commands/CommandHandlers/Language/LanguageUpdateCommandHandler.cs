using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Commands.CommandResults.Language;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using MeetCode.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MeetCode.Application.Commands.CommandHandlers.Language
{
    public class LanguageUpdateCommandHandler : IRequestHandler<LanguageUpdateCommand, Result<LanguageUpdateCommandResult>>
    {
        private readonly ILogger<LanguageUpdateCommandHandler> _logger;
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LanguageUpdateCommandHandler(
            ILogger<LanguageUpdateCommandHandler> logger,
            ILanguageRepository languageRepository,
            IUnitOfWork unitOfWork
            )
        {
            _logger = logger;
            _languageRepository = languageRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<LanguageUpdateCommandResult>> Handle(LanguageUpdateCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to update language {request.LangId}");

            var language = await _languageRepository.GetByIdAsync(request.LangId, ct);
            if (language == null)
            {
                _logger.LogWarning("Language {Id} cannot be found.", request.LangId);
                return Result.Invalid(new ValidationError(nameof(request.LangId), $"Language {request.LangId} cannot be found."));
            }

            var updateObject = new LanguageBasicUpdateObject(
                    request.Name,
                    request.Version,
                    request.FileExtension,
                    request.CompileImage,
                    request.RuntimeImage,
                    request.CompileCommand,
                    request.RunCommand
                );
            language.UpdateBasicInfo(updateObject);

            await _languageRepository.Update(language);
            await _unitOfWork.SaveChangesAsync(ct);

            var result = new LanguageUpdateCommandResult(language);

            _logger.LogInformation($"Language updated successfully to {language.ToGenericString()}");

            return Result.Success(result);
        }
    }
}
