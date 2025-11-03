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
    public class LanguageAllQueryHandler : IRequestHandler<LanguageAllQuery, Result<LanguageAllQueryResult>>
    {
        private readonly ILogger<LanguageAllQueryHandler> _logger;
        private readonly ILanguageService _languageService;
        public LanguageAllQueryHandler(
            ILogger<LanguageAllQueryHandler> logger,
            ILanguageService languageService)
        {
            _logger = logger;
            _languageService = languageService;
        }

        public async Task<Result<LanguageAllQueryResult>> Handle(LanguageAllQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve all languages");

            var languageList = await _languageService.ReadAllLanguagesAsync(ct);

            var result = new LanguageAllQueryResult(languageList);

            _logger.LogInformation($"Retrieve all languages successfully, size of list {languageList.Count()}");
            return Result.Success(result);
        }
    }
}
