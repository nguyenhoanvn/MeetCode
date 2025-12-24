using Ardalis.Result;
using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Application.DTOs.Request.Problem;
using MeetCode.Application.DTOs.Response.Problem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MeetCode.Application.Queries.QueryEntities.Problem;
using Ardalis.Result.AspNetCore;
using MeetCode.Application.Queries.QueryResults.Problem;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("admin/problems")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public ProblemController(
            ISender mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemAddCommandResult), StatusCodes.Status201Created)]
        [ExpectedFailures(ResultStatus.Forbidden, ResultStatus.Conflict, ResultStatus.Error)]
        public async Task<Result<ProblemAddCommandResult>> AddProblem([FromBody] ProblemAddRequest request, CancellationToken ct)
        {
            var cmd = new ProblemAddCommand(
                    request.Title,
                    request.StatementMd,
                    request.Difficulty,
                    request.TimeLimitMs,
                    request.MemoryLimitMb,
                    request.TagIds
                );

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemAllQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemAllQueryResult>> ProblemList(CancellationToken ct)
        {
            var cmd = new ProblemAllQuery();

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("{problemId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemReadQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
        public async Task<Result<ProblemReadQueryResult>> ProblemRead([FromRoute] Guid problemId, CancellationToken ct)
        {
            var cmd = new ProblemReadByIdQuery(problemId);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpPatch("{problemId}/toggle")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemToggleCommandResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<ProblemToggleCommandResult>> ProblemToggle([FromRoute] Guid problemId, CancellationToken ct)
        {
            var cmd = new ProblemToggleCommand(problemId);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpPatch("{problemId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Conflict, ResultStatus.Forbidden, ResultStatus.Error)]
        public async Task<Result<ProblemResponse>> ProblemUpdate([FromRoute] Guid problemId, [FromBody] ProblemUpdateRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<ProblemUpdateCommand>((problemId, request));

            var result = await _mediator.Send(cmd, ct);

            return result.Map(value => _mapper.Map<ProblemResponse>(value));
        }

        [HttpDelete("{problemId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemMessageResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error, ResultStatus.Forbidden)]
        public async Task<Result<ProblemMessageResponse>> ProblemDelete([FromRoute] Guid problemId, [FromBody] ProblemDeleteRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<ProblemDeleteCommand>((problemId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<ProblemMessageResponse>(value));

            return resp;
        }

        [HttpGet("search")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemAllResponse>> ProblemSearch([FromQuery] string name, CancellationToken ct)
        {
            var cmd = new ProblemSearchQuery(name);

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<ProblemAllResponse>(value));

            return resp;
        }
    }
}
