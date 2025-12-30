using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.DTOs.Response.Submit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;
using MeetCode.Application.Queries.QueryEntities.Submission;

namespace MeetCode.Server.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _mediator;
        public UserController(
            ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}/problems/{problemId}/submissions")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(SubmissionAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<SubmissionAllResponse>> GetSubmissions(Guid userId, Guid problemId, CancellationToken ct)
        {
            var cmd = new SubmissionAllUserQuery(userId, problemId);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }
    }
}
