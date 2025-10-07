using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Common;
using Microsoft.Extensions.Logging;
using ReactASP.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http;

namespace ReactASP.Application.Commands.ProblemAdd
{
    public sealed class ProblemAddCommandHandler : IRequestHandler<ProblemAddCommand, Result<ProblemAddResult>>
    {
        private readonly ILogger<ProblemAddCommandHandler> _logger;
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        public ProblemAddCommandHandler(IProblemRepository problemRepository,
            IUnitOfWork unitOfWork,
            ISessionService sessionService,
            ILogger<ProblemAddCommandHandler> logger)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _logger = logger;
        }
        public async Task<Result<ProblemAddResult>> Handle(ProblemAddCommand request, CancellationToken ct)
        {
            try
            {
                var slug = Helpers.GenerateSlug(request.Title);
                var userClaim = _sessionService.GetUserClaim(ct);

                if (userClaim == null || !Guid.TryParse(userClaim.Value, out var userId))
                {
                    _logger.LogWarning("Invalid user Id");
                    return Result.Invalid(new ValidationError
                    {
                        Identifier = nameof(userClaim),
                        ErrorMessage = "Error occured while trying to handle problem add"
                    });
                }

                var newProblem = new Problem
                {
                    ProblemId = Guid.NewGuid(),
                    Slug = slug,
                    Title = request.Title,
                    StatementMd = request.StatementMd,
                    Difficulty = request.Difficulty,
                    TimeLimitMs = request.TimeLimitMs,
                    MemoryLimitMb = request.MemoryLimitMb,
                    CreatedBy = userId,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await _problemRepository.AddAsync(newProblem, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                _logger.LogInformation("New problem added successfully");
                return Result.Success(new ProblemAddResult(newProblem.ProblemId,
                    newProblem.Slug, 
                    newProblem.Title,
                    newProblem.StatementMd,
                    newProblem.Difficulty,
                    newProblem.TimeLimitMs,
                    newProblem.MemoryLimitMb,
                    newProblem.CreatedAt,
                    newProblem.CreatedBy));
            } catch (Exception ex)
            {
                _logger.LogError($"An unexpected exception occured: {ex.Message}");
                return Result.Error("An unexpected exception occured while trying to create new problem");
            }
        }
    }
}
