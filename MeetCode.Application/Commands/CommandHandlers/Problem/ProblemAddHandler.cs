using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces;
using Microsoft.Extensions.Logging;
using MeetCode.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Interfaces.Repositories;

namespace MeetCode.Application.Commands.CommandHandlers.Problem
{
    public sealed class ProblemAddHandler : IRequestHandler<ProblemAddCommand, Result<ProblemAddCommandResult>>
    {
        private readonly ILogger<ProblemAddHandler> _logger;
        private readonly ISessionService _sessionService;
        private readonly IProblemRepository _problemRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ProblemAddHandler(
            ISessionService sessionService,
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork,
            ITagRepository tagRepository,
            ILogger<ProblemAddHandler> logger)
        {
            _sessionService = sessionService;
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _logger = logger;
        }
        public async Task<Result<ProblemAddCommandResult>> Handle(ProblemAddCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to add problem {Request}", request.ToGenericString());

            var userIdResult = _sessionService.ExtractUserIdFromJwt(ct);

            if (userIdResult.IsError())
            {
                _logger.LogWarning("Problem add failed as error occurred while taking userId, error {Error}", userIdResult.Errors);
                return Result.Error(new ErrorList(userIdResult.Errors));
            }
            if (userIdResult.IsUnauthorized())
            {
                _logger.LogWarning("Problem add failed as user is not logged in");
                return Result.Unauthorized("You are not logged in");
            }

            var problem = new MeetCode.Domain.Entities.Problem
            {
                Title = request.Title,
                StatementMd = request.StatementMd,
                Difficulty = request.Difficulty,
                TimeLimitMs = request.TimeLimitMs,
                MemoryLimitMb = request.MemoryLimitMb,
                CreatedBy = userIdResult.Value,
                CreatedAt = DateTimeOffset.UtcNow,
                IsActive = false
            };

            problem.GenerateSlug();

            var tagList = await _tagRepository.GetByIdsAsync(request.TagIds, ct);
            if (tagList.Count() == 0)
            {
                _logger.LogWarning("Problem tag list is empty");
            }

            foreach (var tag in tagList)
            {
                problem.Tags.Add(tag);
            }

            await _problemRepository.AddAsync(problem, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("New problem added successfully");
            return Result.Created(new ProblemAddCommandResult(problem));
        }
    }
}
