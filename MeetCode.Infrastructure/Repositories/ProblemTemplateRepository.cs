using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Domain.Entities;
using MeetCode.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Repositories
{
    public class ProblemTemplateRepository : IProblemTemplateRepository
    {
        private readonly AppDbContext _db;

        public ProblemTemplateRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ProblemTemplate>> GetAsync(CancellationToken ct)
        {
            return await _db.ProblemTemplates
                .Include(pt => pt.Language)
                .Include(pt => pt.Problem)
                .ToListAsync();
        }
        public async Task<ProblemTemplate?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.ProblemTemplates
                .Include(pt => pt.Language)
                .Include(pt => pt.Problem)
                .FirstOrDefaultAsync(pt => pt.TemplateId == id, ct);
        }
        public async Task<IEnumerable<ProblemTemplate>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return await _db.ProblemTemplates
                .Include(pt => pt.Language)
                .Include(pt => pt.Problem)
                .Where(pt => ids.Contains(pt.TemplateId))
                .ToListAsync();
        }
        public async Task AddAsync(ProblemTemplate entity, CancellationToken ct)
        {
            await _db.AddAsync(entity, ct);
        }
        public Task Update(ProblemTemplate newEntity)
        {
            _db.Update(newEntity);
            return Task.CompletedTask;
        }
        public Task Delete(ProblemTemplate entityToDelete)
        {
            _db.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public async Task<bool> IsProblemTemplateExistsAsync(Guid problemId, Guid langId, CancellationToken ct)
        {
            return await _db.ProblemTemplates
                .AsNoTracking()
                .AnyAsync(pt => pt.ProblemId == problemId && pt.LangId == langId);
        }

        public async Task<ProblemTemplate?> GetProblemTemplateByProblemIdAsync(Guid problemId, CancellationToken ct)
        {
            return await _db.ProblemTemplates
                .Include(l => l.Language)
                .Include(p => p.Problem)
                .FirstOrDefaultAsync(pt => pt.ProblemId == problemId, ct);
        }
        public async Task<ProblemTemplate?> GetProblemTemplateByProblemIdLanguageNameAsync(Guid problemId, string languageName, CancellationToken ct)
        {
            return await _db.ProblemTemplates
                .Include(l => l.Language)
                .Include(p => p.Problem)
                .FirstOrDefaultAsync(pt => (pt.ProblemId == problemId) && (pt.Language.Name == languageName), ct);
        }
    }
}
