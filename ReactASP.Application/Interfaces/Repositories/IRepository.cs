using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task Update(T newEntity, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
