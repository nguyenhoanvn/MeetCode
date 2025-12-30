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
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly AppDbContext _db;
        public SubmissionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Submission>> GetAsync(CancellationToken ct)
        {
            return await _db.Submissions
                .Include(s => s.Lang)
                .Include(s => s.Problem)
                .Include(s => s.User)
                .ToListAsync(ct);
        }
        public async Task<Submission?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Submissions
                .Include(s => s.Lang)
                .Include(s => s.Problem)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.SubmissionId == id, ct);
        }
        public async Task<IEnumerable<Submission>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return await _db.Submissions
                .Include(s => s.Lang)
                .Include(s => s.Problem)
                .Include(s => s.User)
                .Where(s => ids.Contains(s.SubmissionId))
                .ToListAsync(ct);
        }
        public async Task AddAsync(Submission entity, CancellationToken ct)
        {
            await _db.AddAsync(entity, ct);
        }
        public Task Update(Submission newEntity)
        {
            _db.Submissions.Update(newEntity);
            return Task.CompletedTask;
        }
        public Task Delete(Submission entityToDelete)
        {
            _db.Submissions.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Submission>> GetAllByUserIdAndProblemIdAsync(Guid userId, Guid problemId, CancellationToken ct)
        {
            return await _db.Submissions
                .Include(s => s.User)
                .Include(s => s.Lang)
                .Include(s => s.Problem)
                .Where(s => s.UserId == userId && s.ProblemId == problemId)
                .ToListAsync(ct);
        }
    }
}
