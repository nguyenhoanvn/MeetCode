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
    public class LanguageRepository : ILanguageRepository
    {
        private readonly AppDbContext _db;
        public LanguageRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Language>> GetAsync(CancellationToken ct)
        {
            return await _db.Languages
                .Include(l => l.Submissions)
                .Include(l => l.ProblemTemplates)
                .ToListAsync(ct);
        }
        public async Task<Language?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Languages
                .Include(l => l.Submissions)
                .Include(l => l.ProblemTemplates)
                .FirstOrDefaultAsync(l => l.LangId == id, ct);
        }
        public async Task<IEnumerable<Language>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return await _db.Languages
                .Include(l => l.Submissions)
                .Include(l => l.ProblemTemplates)
                .Where(l => ids.Contains(l.LangId))
                .ToListAsync(ct);
        }
        public Task AddAsync(Language entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task Update(Language newLanguage)
        {
            _db.Languages.Update(newLanguage);
            return Task.CompletedTask;
        }
        public Task Delete(Language languageToDelete)
        {
            _db.Languages.Remove(languageToDelete);
            return Task.CompletedTask;
        }

        public async Task<Language?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _db.Languages
                .Include(l => l.Submissions)
                .FirstOrDefaultAsync(l => l.Name == name, ct);
        }
    }
}
