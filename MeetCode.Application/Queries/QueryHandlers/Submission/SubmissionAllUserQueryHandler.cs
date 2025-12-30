using Ardalis.Result;
using AutoMapper;
using MediatR;
using MeetCode.Application.DTOs.Response.Submit;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.Submission;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Submission
{
    public class SubmissionAllUserQueryHandler : IRequestHandler<SubmissionAllUserQuery, Result<SubmissionAllResponse>>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly ILogger<SubmissionAllUserQueryHandler> _logger;
        private readonly IMapper _mapper;
        public SubmissionAllUserQueryHandler(
            ISubmissionRepository submissionRepository,
            ILogger<SubmissionAllUserQueryHandler> logger,
            IMapper mapper)
        {
            _submissionRepository = submissionRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<SubmissionAllResponse>> Handle(SubmissionAllUserQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to read submission with user {UserId} and problem {ProblemId}", request.UserId, request.ProblemId);

            var submissions = await _submissionRepository.GetAllByUserIdAndProblemIdAsync(request.UserId, request.ProblemId, ct);

            var submissionDtos = _mapper.Map<List<SubmissionResponse>>(submissions);

            return Result.Success(new SubmissionAllResponse(submissionDtos));
        }
    }
}
