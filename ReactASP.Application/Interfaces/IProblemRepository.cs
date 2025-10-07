using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces
{
    public interface IProblemRepository
    {
        Task AddAsync(Problem problem, CancellationToken ct);
        Task<Problem?> GetBySlugAsync(string slug, CancellationToken ct);
    }
}
