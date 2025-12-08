using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public LanguageService(
            ILanguageRepository languageRepository,
            ILogger<LanguageService> logger,
            IUnitOfWork unitOfWork
            )
        {
            _languageRepository = languageRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Language> UpdateLanguageAsync(string name, string version, string runtimeImage, string compileCommand, string runCommand, CancellationToken ct)
        {
            var language = await FindLanguageByNameAsync(name, ct);
            if (language == null)
            {
                _logger.LogWarning($"Language {name} cannot be found.");
                throw new EntityNotFoundException("Language", nameof(name), name);
            }

            language.Version = version;
            language.RuntimeImage = runtimeImage;
            language.CompileCommand = compileCommand;
            language.RunCommand = runCommand;

            await _languageRepository.Update(language);
            await _unitOfWork.SaveChangesAsync(ct);

            return language;
        }

        public async Task<Language?> FindLanguageByNameAsync(string name, CancellationToken ct)
        {
            return await _languageRepository.GetByNameAsync(name, ct);
        }

        public async Task<Language?> FindLanguageByIdAsync(Guid langId, CancellationToken ct)
        {
            return await _languageRepository.GetByIdAsync(langId, ct);
        }
        public async Task<IEnumerable<Language>> ReadAllLanguagesAsync(CancellationToken ct)
        {
            var languages = await _languageRepository.GetAsync(ct);
            return languages.ToList();
        }
    }
}
