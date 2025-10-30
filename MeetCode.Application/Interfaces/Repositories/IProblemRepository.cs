using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Repositories
{
    public interface IProblemRepository : IRepository<Problem>
    {
        Task<Problem?> GetBySlugAsync(string slug, CancellationToken ct);
        Task<IEnumerable<Problem>> GetAllBySlugAsync(string slug, CancellationToken ct);
        Task<bool> IsProblemExistsAsync(string slug, CancellationToken ct);
    }
}
