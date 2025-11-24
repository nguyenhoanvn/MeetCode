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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemTemplateService> _logger;
        public ProblemTemplateService(
            IProblemTemplateRepository problemTemplateRepository,
            IUnitOfWork unitOfWork,
            ILogger<ProblemTemplateService> logger)
        {
            _problemTemplateRepository = problemTemplateRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProblemTemplate> CreateTemplateAsync(string methodName, string returnType, string[] parameters, Guid problemId, Guid langId, CancellationToken ct)
        { 
            if (await _problemTemplateRepository.IsProblemTemplateExistsAsync(problemId, langId, ct))
            {
                _logger.LogWarning($"Template associated with problemId {problemId} and langId {langId} exists");
                throw new DuplicateEntityException<ProblemTemplate>(new Dictionary<string, string>
                {
                    {nameof(problemId), problemId.ToString() },
                    {nameof(langId), langId.ToString() }
                });
            }
            var methodSignature = GenerateMethodSignature(methodName, returnType, parameters);

            var problemTemplate = new ProblemTemplate
            {
                ProblemId = problemId,
                LangId = langId,
                TemplateCode = methodSignature
            };

            await _problemTemplateRepository.AddAsync(problemTemplate, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return problemTemplate;
        }
        public async Task<ProblemTemplate?> FindTemplateByIdAsync(Guid templateId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<ProblemTemplate>> ReadAllTemplatesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public string GenerateMethodSignature(string methodName, string returnType, string[] parameterNames)
        {
            var joined = string.Join(", ", parameterNames);
            return $"public {returnType} {methodName}({joined}){{\n}}";
        }
    }
}
