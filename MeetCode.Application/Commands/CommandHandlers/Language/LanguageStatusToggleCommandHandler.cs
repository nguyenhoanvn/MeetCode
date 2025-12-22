using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Commands.CommandResults.Language;
using MeetCode.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Language
{
    public class LanguageStatusToggleCommandHandler : IRequestHandler<LanguageStatusToggleCommand, Result<LanguageStatusToggleCommandResult>>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageStatusToggleCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public LanguageStatusToggleCommandHandler(
            ILanguageRepository languageRepository,
            ILogger<LanguageStatusToggleCommandHandler> logger,
            IUnitOfWork unitOfWork
            )
        {
            _languageRepository = languageRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<LanguageStatusToggleCommandResult>> Handle(LanguageStatusToggleCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to toggle status for language {LangId}", request.LangId);

            var language = await _languageRepository.GetByIdAsync(request.LangId, ct);

            if (language == null)
            {
                _logger.LogWarning("Language {Id} cannot be found.", request.LangId);
                return Result.Invalid(new ValidationError(nameof(request.LangId), $"Language {request.LangId} cannot be found."));
            }

            language.ToggleStatus();

            await _languageRepository.Update(language);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Language {LangId} changed to {Status}", request.LangId, language.IsEnabled);
            return Result.Success(new LanguageStatusToggleCommandResult(language));
        }
    }
}
