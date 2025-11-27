using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Submit
{
    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, Result<EnqueueResult>>
    {
        private readonly IJobSenderService _jobSender;
        private readonly ILogger<RunCodeCommandHandler> _logger;
        public RunCodeCommandHandler(
            IJobSenderService jobSender,
            ILogger<RunCodeCommandHandler> logger)
        {
            _jobSender = jobSender;
            _logger = logger;
        }
        public async Task<Result<EnqueueResult>> Handle(RunCodeCommand request, CancellationToken ct)
        {
            try
            {
                await _jobSender.EnqueueJobAsync<RunCodeCommand>(request, "run_code_queue", ct);
                var result = new EnqueueResult(request, "run_code_queue");
                return Result.Success(result);
            } catch (Exception ex)
            {
                return Result.Error("Failed to enqueue message:" + ex.Message);
            }
        }
    }
}
