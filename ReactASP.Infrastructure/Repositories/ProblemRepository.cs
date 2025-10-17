using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactASP.Application.Interfaces;
using ReactASP.Domain.Entities;
using ReactASP.Infrastructure.Persistence;

namespace ReactASP.Infrastructure.Repositories
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
        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public async Task<Problem?> GetBySlugAsync(string slug, CancellationToken ct)
        {
            return await _db.Problems.FirstOrDefaultAsync(p => p.Slug == slug, ct);
        }
    }
}
