using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Repositories
{
    public interface IProblemTemplateRepository : IRepository<ProblemTemplate>
    {
        Task<bool> IsProblemTemplateExistsAsync(Guid problemId, Guid langId, CancellationToken ct);
        Task<ProblemTemplate?> GetProblemTemplateByProblemIdAsync(Guid problemId, CancellationToken ct);
    }
}
