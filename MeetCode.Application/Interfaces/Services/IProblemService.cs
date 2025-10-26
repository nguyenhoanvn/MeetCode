using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IProblemService
    {
        Task<Problem> CreateProblemAsync(string title, string statementMd, string difficulty, int timeLimitMs, int memoryLimitMb, Guid createdBy, List<Guid> tagIds, CancellationToken ct);
        Task<IEnumerable<Problem>> ReadAllProblemsAsync(CancellationToken ct);
        Task<Problem?> FindProblemByIdAsync(Guid problemId, CancellationToken ct);
        Task<Problem?> FindProblemBySlugAsync(string problemSlug, CancellationToken ct);
        Task<Problem?> UpdateProblemAsync(ProblemUpdateCommand problem, CancellationToken ct);
        Task DeleteProblemAsync(Problem problemToDelete, CancellationToken ct);
        Task<IEnumerable<Problem>> ReadAllProblemsBySlugAsync(string slug, CancellationToken ct);
        
    }
}
