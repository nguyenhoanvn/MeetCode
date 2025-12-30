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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Problem
{
    public class ProblemSearchQueryUserHandler : IRequestHandler<ProblemSearchQueryUser, Result<ProblemAllResponse>>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly ILogger<ProblemSearchQueryUserHandler> _logger;
        private readonly IMapper _mapper;
        public ProblemSearchQueryUserHandler(
            IProblemRepository problemRepository,
            ILogger<ProblemSearchQueryUserHandler> logger,
            IMapper mapper)
        {
            _problemRepository = problemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<ProblemAllResponse>> Handle(ProblemSearchQueryUser request, CancellationToken ct)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var (items, totalCount) = await _problemRepository.GetByTitlePagedAsync(pageNumber, pageSize, request.Title, ct);

            if (totalCount == 0)
            {
                _logger.LogWarning("Problem list is empty");
            }

            var response = new ProblemAllResponse(
                items.Select(p => _mapper.Map<ProblemResponse>(p)).ToList(),
                pageNumber,
                pageSize,
                totalCount,
                (int)Math.Ceiling(totalCount / (double)pageSize)
                );

            return Result.Success(response);
        }
    }
}
