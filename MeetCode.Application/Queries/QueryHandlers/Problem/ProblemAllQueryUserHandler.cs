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
    public class ProblemAllQueryUserHandler : IRequestHandler<ProblemAllQueryUser, Result<ProblemAllResponse>>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly ILogger<ProblemAllQueryUserHandler> _logger;
        private readonly IMapper _mapper;
        public ProblemAllQueryUserHandler(
            IProblemRepository problemRepository,
            ILogger<ProblemAllQueryUserHandler> logger,
            IMapper mapper)
        {
            _problemRepository = problemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<ProblemAllResponse>> Handle(ProblemAllQueryUser request, CancellationToken ct)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var (items, totalCount) = await _problemRepository.GetPagedAsync(pageNumber, pageSize, ct);

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
