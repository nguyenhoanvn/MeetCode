using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces.Services
{
    public interface IProblemService
    {
        Task<Problem> CreateProblemAsync(string title, string statementMd, string difficulty, int timeLimitMs, int memoryLimitMb, Guid createdBy, CancellationToken ct);
        Task<IEnumerable<Problem>> ReadAllProblemsAsync(CancellationToken ct);
        Task<Problem?> FindProblemByIdAsync(Guid problemId, CancellationToken ct);
        
    }
}
