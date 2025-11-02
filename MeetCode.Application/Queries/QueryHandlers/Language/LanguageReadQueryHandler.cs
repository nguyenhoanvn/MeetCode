using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Language;
using MeetCode.Application.Queries.QueryResults.Language;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Language
{
    public class LanguageReadQueryHandler : IRequestHandler<LanguageReadQuery, Result<LanguageReadQueryResult>>
    {
        private readonly ILanguageService _languageService;
        private readonly ILogger<LanguageReadQueryHandler> _logger;
        public LanguageReadQueryHandler(
            ILanguageService languageService,
            ILogger<LanguageReadQueryHandler> logger)
        {
            _languageService = languageService;
            _logger = logger;
        }

        public async Task<Result<LanguageReadQueryResult>> Handle(LanguageReadQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to read language {request.LangId}");

            var language = await _languageService.FindLanguageByIdAsync(request.LangId, ct);
            if (language == null)
            {
                _logger.LogWarning($"Cannot find language with id {request.LangId}");
                return Result.NotFound($"Cannot find language with id {request.LangId}");
            }

            var result = new LanguageReadQueryResult(language);
            return Result.Success(result);
        }
    }
}
