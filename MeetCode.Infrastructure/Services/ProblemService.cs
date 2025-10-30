using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository _problemRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProblemService> _logger;
        public ProblemService(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork,
            ITagRepository tagRepository,
            ILogger<ProblemService> logger)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _tagRepository = tagRepository;
        }
        public async Task<Problem> CreateProblemAsync(string title, string statementMd, string difficulty, int timeLimitMs, int memoryLimitMb, Guid userId, List<Guid> tagIds, CancellationToken ct)
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

            // Check existing
            var existing = await FindProblemBySlugAsync(newProblem.Slug, ct);
            if (existing != null)
            {
                _logger.LogWarning($"Problem {title} already in database");
                throw new DuplicateEntityException<Problem>(new Dictionary<string, string>
                {
                    {nameof(newProblem.Title), newProblem.Title }
                });
            }

            // Find tags
            IEnumerable<ProblemTag> tagList = new List<ProblemTag>();
            tagList = await _tagRepository.GetByIdsAsync(tagIds, ct);
            foreach(var tag in tagList)
            {
                newProblem.Tags.Add(tag);
            }

            await _problemRepository.AddAsync(newProblem, ct);

            // Check saved
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                _logger.LogWarning($"Failed to add problem {title} to the database");
                throw new DbUpdateException("Failed to save the new problem to the database.");
            }
            _logger.LogInformation($"Problem created successfully: {newProblem.ToString()}");
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

        public async Task<Problem> UpdateProblemAsync(Guid problemId, string title, string statementMd, string difficulty, IEnumerable<Guid> tagIds, CancellationToken ct)
        {
            var problem = await _problemRepository.GetByIdAsync(problemId, ct);
            if (problem == null)
            {
                _logger.LogWarning($"Cannot find problem with id {problemId}");
                throw new EntityNotFoundException<Problem>(nameof(problemId), problemId.ToString());
            }

            problem.UpdateBasic(title, statementMd, difficulty);
            if (await _problemRepository.IsProblemExistsAsync(problem.Slug, ct))
            {
                _logger.LogWarning($"Problem with slug {problem.Slug} already exists");
                throw new DuplicateEntityException<Problem>(new Dictionary<string, string>
                {
                    {nameof(problem.Slug), problem.Slug }
                });
            }
            var tagList = await _tagRepository.GetByIdsAsync(tagIds, ct);
            problem.UpdateTags(tagList);

            await _unitOfWork.BeginTransactionAsync(ct);
            await _problemRepository.Update(problem);
            await _unitOfWork.CommitTransactionAsync(ct);
            return problem;
        }

        public async Task DeleteProblemAsync(Problem problemToDelete, CancellationToken ct)
        {
            await _problemRepository.Delete(problemToDelete);
            var saved = await _unitOfWork.SaveChangesAsync(ct);
            if (saved <= 0)
            {
                _logger.LogWarning($"Failed to delete problem {problemToDelete.Title}");
                throw new DbUpdateException("Failed to save the new problem to the database.");
            }
        }

        public async Task<IEnumerable<Problem>> ReadAllProblemsBySlugAsync(string slug, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve all problems with slug {slug}");
            return await _problemRepository.GetAllBySlugAsync(slug, ct);
        }
    }
}
