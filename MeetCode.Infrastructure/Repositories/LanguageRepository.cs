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
        public Task Update(Language newLanguage)
        {
            _db.Languages.Update(newLanguage);
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
