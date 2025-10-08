using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Application.Interfaces.Repositories;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces
{
    public interface IProblemRepository : IRepository<Problem>
    {
        Task<Problem?> GetBySlugAsync(string slug, CancellationToken ct);
    }
}
