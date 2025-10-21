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
    public class ProblemRepository : IProblemRepository
    {
        private readonly AppDbContext _db;

        public ProblemRepository(AppDbContext db)
        {
            _db = db;
        } 

        public async Task<IEnumerable<Problem>> GetAsync(CancellationToken ct)
        {
            return await _db.Problems.ToListAsync(ct);
        }
        public async Task<Problem?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Problems.FindAsync(id, ct);
        }
        public async Task AddAsync(Problem problem, CancellationToken ct)
        {
            await _db.Problems.AddAsync(problem, ct);
        }
        public async Task Update(Problem newProblem, CancellationToken ct)
        {
            _db.Problems.Update(newProblem);
            await Task.CompletedTask;
        }
        public async Task Delete(Problem problemToDelete, CancellationToken ct)
        {
            _db.Problems.Remove(problemToDelete);
            await Task.CompletedTask;
        }
        public async Task<Problem?> GetBySlugAsync(string slug, CancellationToken ct)
        {
            return await _db.Problems.FirstOrDefaultAsync(p => p.Slug == slug, ct);
        }
    }
}
