using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Commands.CommandResults.Language;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.Language
{
    public class LanguageUpdateCommandHandler : IRequestHandler<LanguageUpdateCommand, Result<LanguageUpdateCommandResult>>
    {
        private readonly ILogger<LanguageUpdateCommandHandler> _logger;
        private readonly ILanguageService _languageService;
        public LanguageUpdateCommandHandler(
            ILogger<LanguageUpdateCommandHandler> logger,
            ILanguageService languageService
            )
        {
            _logger = logger;
            _languageService = languageService;
        }
        public async Task<Result<LanguageUpdateCommandResult>> Handle(LanguageUpdateCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to update language {request.Name}");

            var updatedLanguage = await _languageService.UpdateLanguageAsync(
                request.Name,
                request.Version,
                request.RuntimeImage,
                request.CompileCommand,
                request.RunCommand,
                ct
                );

            var result = new LanguageUpdateCommandResult(updatedLanguage);

            _logger.LogInformation($"Language updated successfully to {updatedLanguage.ToString()}");

            return result;
        }
    }
}
