using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Domain.Entities;
using MeetCode.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MeetCode.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _db;
        public TagRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ProblemTag>> GetAsync(CancellationToken ct)
        {
            return await _db.ProblemTags
                .Include(pt => pt.Problems)
                .ToListAsync();
        }
        public async Task<ProblemTag?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var tag = await _db.ProblemTags
                .Include(pt => pt.Problems)
                .FirstOrDefaultAsync(pt => pt.TagId == id, ct);
            return tag;
        }
        public async Task<IEnumerable<ProblemTag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return await _db.ProblemTags
                .Include(pt => pt.Problems)
                .Where(p => ids.Contains(p.TagId)).ToListAsync(ct);
        }
        public async Task AddAsync(ProblemTag entity, CancellationToken ct)
        {
            await _db.ProblemTags.AddAsync(entity, ct);
        }

        public Task Update(ProblemTag newEntity, CancellationToken ct)
        {
            _db.ProblemTags.Update(newEntity);
            return Task.CompletedTask;
        }
        public Task Delete(ProblemTag entityToDelete, CancellationToken ct)
        {
            _db.ProblemTags.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public async Task<ProblemTag?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _db.ProblemTags.FirstOrDefaultAsync(t => t.Name == name, ct);
        }
    }
}
