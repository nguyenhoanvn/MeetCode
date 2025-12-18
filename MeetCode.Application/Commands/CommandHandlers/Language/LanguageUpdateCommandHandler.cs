using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Commands.CommandResults.Language;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
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
            _logger.LogInformation($"Attempting to update language {request.Name}");

            var language = await _languageRepository.GetByNameAsync(request.Name, ct);
            if (language == null)
            {
                _logger.LogWarning("Language {Name} cannot be found.", request.Name);
                return Result.Invalid(new ValidationError(nameof(request.Name), $"Language {request.Name} cannot be found."));
            }

            language.Version = request.Version;
            language.RuntimeImage = request.RuntimeImage;
            language.CompileCommand = request.CompileCommand;
            language.RunCommand = request.RunCommand;

            await _languageRepository.Update(language);
            await _unitOfWork.SaveChangesAsync(ct);

            var result = new LanguageUpdateCommandResult(language);

            _logger.LogInformation($"Language updated successfully to {language.ToGenericString()}");

            return Result.Success(result);
        }
    }
}
