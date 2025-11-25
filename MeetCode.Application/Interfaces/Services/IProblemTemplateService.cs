using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IProblemTemplateService
    {
        Task<ProblemTemplate> CreateTemplateAsync(string methodName, string returnType, string[] parameters, Guid problemId, Guid langId, CancellationToken ct);
        Task<ProblemTemplate?> FindTemplateByIdAsync(Guid templateId, CancellationToken ct);
        Task<IEnumerable<ProblemTemplate>> ReadAllTemplatesAsync(CancellationToken ct);
        Task<string> GenerateMethodSignature(string methodName, string returnType, string[] parameters, Guid langId, CancellationToken ct);
    }
}
