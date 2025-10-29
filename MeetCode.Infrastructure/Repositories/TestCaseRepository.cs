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
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly AppDbContext _db;
        public TestCaseRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<TestCase>> GetAsync(CancellationToken ct)
        {
            return await _db.TestCases
                .Include(x => x.Problem)
                .ToListAsync();
        }
        public async Task<TestCase?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.TestCases
                .Include(x => x.Problem)
                .FirstOrDefaultAsync(x => x.TestId == id, ct);
        }
        public async Task<IEnumerable<TestCase>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return await _db.TestCases
                .Include(tc => tc.Problem)
                .Where(tc => ids.Contains(tc.TestId))
                .ToListAsync();
        }
        public async Task AddAsync(TestCase entity, CancellationToken ct)
        {
            await _db.TestCases.AddAsync(entity, ct);
        }
        public Task Update(TestCase newEntity)
        {
            _db.TestCases.Update(newEntity);
            return Task.CompletedTask;
        }
        public Task Delete(TestCase entityToDelete)
        {
            _db.TestCases.Remove(entityToDelete);
            return Task.CompletedTask;
        }
        public async Task<TestCase?> GetByInputOutputAndProblemAsync(string inputText, string outputText, Guid problemId, CancellationToken ct)
        {
            return await _db.TestCases
                .Include(tc => tc.Problem)
                .FirstOrDefaultAsync((tc => 
                tc.InputText.ToLower() == inputText.ToLower()
                && tc.ExpectedOutputText.ToLower() == outputText.ToLower()
                && tc.ProblemId == problemId), ct);
        }
        public async Task<bool> IsTestCaseExistsAsync(string inputText, string outputText, Guid problemId, CancellationToken ct)
        {
            return await _db.TestCases
                .AsNoTracking()
                .AnyAsync(tc => tc.InputText.ToLower() == inputText.ToLower()
                && tc.ExpectedOutputText.ToLower() == outputText.ToLower()
                && tc.ProblemId == problemId, ct);
        }
    }
}
