using Ardalis.Result;
using AutoMapper;
using MediatR;
using MeetCode.Application.DTOs.Response.Problem;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.Problem;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Problem
{
    public class ProblemReadBySlugQueryUserHandler : IRequestHandler<ProblemReadBySlugQueryUser, Result<ProblemResponse>>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly ILogger<ProblemReadBySlugQueryUserHandler> _logger;
        private readonly IMapper _mapper;
        public ProblemReadBySlugQueryUserHandler(
            IProblemRepository problemRepository,
            ILogger<ProblemReadBySlugQueryUserHandler> logger,
            IMapper mapper
            )
        {
            _problemRepository = problemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<ProblemResponse>> Handle(ProblemReadBySlugQueryUser request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to get problem by slug {Slug}", request.Slug);

            var problem = await _problemRepository.GetBySlugAsync(request.Slug, ct);

            if (problem == null)
            {
                _logger.LogWarning("Cannot find problem {Slug}", request.Slug);
                return Result.Invalid(new ValidationError(nameof(request.Slug), $"Cannot find problem {request.Slug}"));
            }

            var problemResponse = _mapper.Map<ProblemResponse>(problem);

            return Result.Success(problemResponse);
        }
    }
}
