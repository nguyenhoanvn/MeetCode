using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Http;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using MeetCode.Application.DTOs.Request.Submit;
using MediatR;
using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Commands.CommandEntities.Job;

namespace MeetCode.Server.Controllers
{
    [Route("submit")]
    [ApiController]
    public class SubmitController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public SubmitController(
            ISender mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("job/run")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(EnqueueResult), StatusCodes.Status202Accepted)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<EnqueueResult>> RunCode(RunCodeRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<RunCodeJobCommand>(request);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }
    }
}
