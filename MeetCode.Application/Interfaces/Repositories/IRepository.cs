using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(CancellationToken ct);
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task Update(T newEntity, CancellationToken ct);
        Task Delete(T entityToDelete, CancellationToken ct);
    }
}
