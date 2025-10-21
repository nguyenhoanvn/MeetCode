using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Repositories
{
    public interface ITagRepository : IRepository<ProblemTag>
    {
        Task<ProblemTag?> GetByNameAsync(string name, CancellationToken ct);
    }
}
