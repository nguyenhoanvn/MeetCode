using Ardalis.Result;
using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Server.DTOs.Request.Problem;
using MeetCode.Server.DTOs.Response.Problem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MeetCode.Application.Queries.QueryEntities.Problem;
using Ardalis.Result.AspNetCore;

namespace MeetCode.Server.Controllers
{
    [Route("problems")]
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
        [ProducesResponseType(typeof(ProblemResponse), StatusCodes.Status201Created)]
        [ExpectedFailures(ResultStatus.Forbidden, ResultStatus.Conflict, ResultStatus.Error)]
        public async Task<Result<ProblemResponse>> AddProblem([FromBody] ProblemAddRequest request, CancellationToken ct)
        {     
            var cmd = _mapper.Map<ProblemAddCommand>(request);

            var result = await _mediator.Send(cmd, ct);

            return result.Map(value => _mapper.Map<ProblemResponse>(value));
        }

        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemAllResponse>> ProblemList(CancellationToken ct)
        {
            var cmd = new ProblemAllQuery();

            var result = await _mediator.Send(cmd, ct);

            return result.Map(value => _mapper.Map<ProblemAllResponse>(value));
        }

        [HttpGet("{slug}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
        public async Task<Result<ProblemResponse>> ProblemRead([FromRoute]string slug, CancellationToken ct)
        {
            var cmd = new ProblemReadQuery(slug);

            var result = await _mediator.Send(cmd, ct);

            return result.Map(value => _mapper.Map<ProblemResponse>(value));
        }

        [HttpPut("{problemId}")]
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
    }
}
