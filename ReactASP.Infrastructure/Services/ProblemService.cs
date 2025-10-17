using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Data.SqlClient;
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
            newProblem.GenerateSlug();
            _logger.LogInformation($"Problem created successfully: {newProblem.ToString()}");
            await _problemRepository.AddAsync(newProblem, ct);

            // Check saved
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                _logger.LogWarning($"Failed to add problem {title} to the database");
                throw new InvalidOperationException("Failed to save the new problem to the database.");
            }
            return newProblem;
        }
        public async Task<IEnumerable<Problem>> ReadAllProblemsAsync(CancellationToken ct)
        {
            _logger.LogInformation("Attempting to retrieve all problems");
            return await _problemRepository.GetAsync(ct);
        }
        public async Task<Problem?> FindProblemByIdAsync(Guid problemId, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve problem with id: {problemId}");
            return await _problemRepository.GetByIdAsync(problemId, ct);
        }

        public async Task<Problem?> FindProblemBySlugAsync(string problemSlug, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve problem with slug: {problemSlug}");
            return await _problemRepository.GetBySlugAsync(problemSlug, ct);
        }

        public async Task<Problem?> UpdateProblemAsync(Problem newProblem, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to update problem to {newProblem.ToString}");
            await _problemRepository.Update(newProblem, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation($"Update problem successfully to {newProblem.ToString}");
            return newProblem;
        }

        public async Task DeleteProblemAsync(Problem problemToDelete, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to delete problem: {problemToDelete}");
            await _problemRepository.DeleteAsync(problemToDelete, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation($"Delete successfully for {problemToDelete}");
        }
    }
}
