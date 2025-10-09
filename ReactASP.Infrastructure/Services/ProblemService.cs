using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;

namespace ReactASP.Infrastructure.Services
{
    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemService> _logger;
        public ProblemService(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork,
            ILogger<ProblemService> logger)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Problem> CreateProblemAsync(string title, string statementMd, string difficulty, int timeLimitMs, int memoryLimitMb, Guid userId, CancellationToken ct)
        {
            _logger.LogInformation($"Create problem function started for {title}");
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
            _logger.LogInformation($"Problem created successfully: {newProblem.ToString()}");
            await _problemRepository.AddAsync(newProblem, ct);

            // Check saved
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                _logger.LogWarning($"Failed to add problem {title} to the database");
                throw new InvalidOperationException("Failed to add new problem to the database");
            }
            return newProblem;
        }
    }
}
