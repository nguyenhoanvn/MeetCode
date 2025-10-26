﻿using System;
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
            return await _db.Problems
                .Include(p => p.Tags)
                .Include(p => p.TestCases)
                .Include(p => p.Submissions)
                .ToListAsync(ct);
        }
        public async Task<Problem?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var problem = await _db.Problems
                .Include(p => p.Tags)
                .Include(p => p.TestCases)
                .Include(p => p.Submissions)
                .FirstOrDefaultAsync(p => p.ProblemId == id, ct);
            return problem;
        }

        public async Task<IEnumerable<Problem>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            throw new NotImplementedException();
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
            return await _db.Problems
                .Include(p => p.Tags)
                .Include(p => p.TestCases)
                .Include(p => p.Submissions)
                .FirstOrDefaultAsync(p => p.Slug == slug, ct);
        }
        public async Task<IEnumerable<Problem>> GetAllBySlugAsync(string slug, CancellationToken ct)
        {
            return await _db.Problems
                .Include(p => p.Tags)
                .Include(p => p.TestCases)
                .Include(p => p.Submissions)
                .Where(p => p.Slug.Contains(slug)).ToListAsync();
        }
    }
}
