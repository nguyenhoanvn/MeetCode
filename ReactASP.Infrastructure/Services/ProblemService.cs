using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;

namespace ReactASP.Infrastructure.Services
{
    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProblemService(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Problem> CreateProblemAsync(string title, string statementMd, string difficulty, int timeLimitMs, int memoryLimitMb, Guid userId, CancellationToken ct)
        {
            // Create new problem
            var newProblem = new Problem
            {
                ProblemId = Guid.NewGuid(),
                Title = title,
                StatementMd = statementMd,
                Difficulty = difficulty,
                TimeLimitMs = timeLimitMs,
                MemoryLimitMb = memoryLimitMb,
                CreatedBy = userId,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await _problemRepository.AddAsync(newProblem, ct);

            // Check saved
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                throw new InvalidOperationException("Failed to add new refresh token");
            }
            return newProblem;
        }
    }
}
