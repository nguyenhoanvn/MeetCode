using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Language;
using MeetCode.Application.Queries.QueryResults.Language;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Language
{
    public class LanguageAllQueryHandler : IRequestHandler<LanguageAllQuery, Result<LanguageAllQueryResult>>
    {
        private readonly ILogger<LanguageAllQueryHandler> _logger;
        private readonly ILanguageRepository _languageRepository;
        public LanguageAllQueryHandler(
            ILogger<LanguageAllQueryHandler> logger,
            ILanguageRepository languageRepository)
        {
            _logger = logger;
            _languageRepository = languageRepository;
        }

        public async Task<Result<LanguageAllQueryResult>> Handle(LanguageAllQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve all languages");

            var languageList = (await _languageRepository.GetAsync(ct)).ToList();

            var result = new LanguageAllQueryResult(languageList);

            _logger.LogInformation($"Retrieve all languages successfully, size of list {languageList.Count()}");
            return Result.Success(result);
        }
    }
}
