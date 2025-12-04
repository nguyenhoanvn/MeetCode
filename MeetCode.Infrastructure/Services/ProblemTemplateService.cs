using MeetCode.Application.Interfaces.Helpers;
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

namespace MeetCode.Infrastructure.Services
{
    public class ProblemTemplateService : IProblemTemplateService
    {
        private readonly IProblemTemplateRepository _problemTemplateRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemTemplateService> _logger;

        private readonly Dictionary<string, ILanguageTemplateGenerator> _templateMap;

        public ProblemTemplateService(
            IProblemTemplateRepository problemTemplateRepository,
            ILanguageRepository languageRepository,
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork,
            IEnumerable<ILanguageTemplateGenerator> generators,
            ILogger<ProblemTemplateService> logger)
        {
            _problemTemplateRepository = problemTemplateRepository;
            _languageRepository = languageRepository;
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
            _templateMap = generators.ToDictionary(g => g.LanguageName.ToLower());
            _logger = logger;
        }

        public async Task<ProblemTemplate> CreateTemplateAsync(string methodName, string returnType, string[] parameters, Guid problemId, Guid langId, CancellationToken ct)
        { 
            // Check foreign records existence
            var problem = await _problemRepository.GetByIdAsync(problemId, ct);
            if (problem == null)
            {
                _logger.LogWarning($"No problem with Id {problemId} found");
                throw new EntityNotFoundException("Problem", nameof(problemId), problemId.ToString());
            }

            var language = await _languageRepository.GetByIdAsync(langId, ct);
            if (language == null)
            {
                _logger.LogWarning($"No language with Id {langId} found");
                throw new EntityNotFoundException("Language", nameof(langId), langId.ToString());
            }

            if (await _problemTemplateRepository.IsProblemTemplateExistsAsync(problemId, langId, ct))
            {
                _logger.LogWarning($"Template associated with problem {problem.Title} and language {language.Name} exists");
                throw new DuplicateEntityException<ProblemTemplate>(new Dictionary<string, string>
                {
                    {nameof(problemId), problemId.ToString() },
                    {nameof(langId), langId.ToString() }
                });
            }

            if (!_templateMap.TryGetValue(language.Name.ToLower(), out var generator))
            {
                throw new NotSupportedException($"Template generator for '{language.Name}' not supported");
            }

            var templateCode = generator.GenerateTemplate(methodName, returnType, parameters);

            var problemTemplate = new ProblemTemplate
            {
                ProblemId = problemId,
                LangId = langId,
                TemplateCode = templateCode
            };

            await _problemTemplateRepository.AddAsync(problemTemplate, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return problemTemplate;
        }
        public async Task<ProblemTemplate?> FindTemplateBySlugAsync(string problemSlug, CancellationToken ct)
        {
            var problem = await _problemRepository.GetBySlugAsync(problemSlug, ct);
            if (problem == null)
            {
                _logger.LogWarning($"Problem with slug {problemSlug} not found");
                throw new EntityNotFoundException("Problem", nameof(problemSlug), problemSlug);
            }

            return await _problemTemplateRepository.GetProblemTemplateByProblemIdAsync(problem.ProblemId, ct);
        }
        public async Task<IEnumerable<ProblemTemplate>> ReadAllTemplatesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
