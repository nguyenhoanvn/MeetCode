using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using AutoMapper;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Language;
using MeetCode.Application.Queries.QueryResults.Language;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Language
{
    public class LanguageReadQueryHandler : IRequestHandler<LanguageReadQuery, Result<LanguageReadQueryResult>>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageReadQueryHandler> _logger;
        public LanguageReadQueryHandler(
            ILanguageRepository languageRepository,
            ILogger<LanguageReadQueryHandler> logger)
        {
            _languageRepository = languageRepository;
            _logger = logger;
        }

        public async Task<Result<LanguageReadQueryResult>> Handle(LanguageReadQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to read language {LangId}", request.LangId);

            var language = await _languageRepository.GetByIdAsync(request.LangId, ct);
            if (language == null)
            {
                _logger.LogWarning($"Cannot find language with Id {request.LangId}");
                return Result.Invalid(new ValidationError (nameof(language), $"Cannot find language with Id {request.LangId}"));
            }

            var result = new LanguageReadQueryResult(language);
            return Result.Success(result);
        }
    }
}
