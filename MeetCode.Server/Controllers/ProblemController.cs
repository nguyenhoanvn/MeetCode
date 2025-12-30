using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using MeetCode.Application.DTOs.Request.Problem;
using MeetCode.Application.DTOs.Response.Problem;
using MeetCode.Application.Queries.QueryEntities.Problem;
using MeetCode.Application.Queries.QueryResults.Problem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Server.Controllers
{
    [Route("problems")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly int Default_Page_Number = 1;
        private readonly int Default_Page_Size = 20;
        public ProblemController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemAllResponse>> ProblemList([FromQuery] ProblemListRequest request, CancellationToken ct)
        {
            var cmd = new ProblemAllQueryUser(
                request.PageNumber ?? Default_Page_Number,
                request.PageSize ?? Default_Page_Size
                );

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("{slug}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemResponse>> ProblemRead([FromRoute] string slug, CancellationToken ct)
        {
            var cmd = new ProblemReadBySlugQueryUser(slug);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("search")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemAllResponse>> ProblemSearch([FromQuery] string title, CancellationToken ct)
        {
            var cmd = new ProblemSearchQueryUser(title);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }
    }
}
