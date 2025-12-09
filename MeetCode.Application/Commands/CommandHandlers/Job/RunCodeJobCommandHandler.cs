using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Job;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Job
{
    public class RunCodeJobCommandHandler : IRequestHandler<RunCodeJobCommand, Result<EnqueueResult<RunCodeJobCommand>>>
    {
        private readonly IJobSender _jobSender;
        private readonly ILogger<RunCodeJobCommandHandler> _logger;
        public RunCodeJobCommandHandler(
            IJobSender jobSender,
            ILogger<RunCodeJobCommandHandler> logger)
        {
            _jobSender = jobSender;
            _logger = logger;
        }
        public async Task<Result<EnqueueResult<RunCodeJobCommand>>> Handle(RunCodeJobCommand request, CancellationToken ct)
        {
            try
            {
                await _jobSender.EnqueueJobAsync<RunCodeJobCommand>(request, "run_code_queue", ct);
                var result = new EnqueueResult<RunCodeJobCommand>(request.JobId, "queued", request, "run_code_queue");
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Error("Failed to enqueue message:" + ex.Message);
            }
        }
    }
}
