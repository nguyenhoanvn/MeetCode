using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.DTOs.Response.TestCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;
using MeetCode.Application.Commands.CommandEntities.TestCase;
using MeetCode.Application.DTOs.Request.TestCase;
using MeetCode.Application.Queries.QueryEntities.TestCase;

namespace MeetCode.Server.Controllers
{
    [Route("test-cases")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public TestCaseController(
            ISender mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost("{problemId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TestCaseResponse), StatusCodes.Status201Created)]
        [ExpectedFailures(ResultStatus.Forbidden, ResultStatus.Conflict, ResultStatus.Error)]
        public async Task<Result<TestCaseResponse>> TestCaseCreate([FromRoute] Guid problemId, [FromBody] TestCaseAddRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TestCaseAddCommand>((problemId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TestCaseResponse>(value));

            return resp;
        }

        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TestCaseAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<TestCaseAllResponse>> TestCaseList(CancellationToken ct)
        {
            var request = new TestCaseAllRequest();

            var cmd = _mapper.Map<TestCaseAllQuery>(request);

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TestCaseAllResponse>(value));

            return resp;
        }

        [HttpGet("{testId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TestCaseResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
        public async Task<Result<TestCaseResponse>> TestCaseRead([FromRoute] Guid testId, CancellationToken ct)
        {
            var request = new TestCaseReadRequest();

            var cmd = _mapper.Map<TestCaseReadQuery>((testId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TestCaseResponse>(value));

            return resp;
        }

        [HttpPut("{testId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TestCaseResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
        public async Task<Result<TestCaseResponse>> TestCaseUpdate([FromRoute] Guid testId, TestCaseUpdateRequest request, CancellationToken ct) 
        {
            var cmd = _mapper.Map<TestCaseUpdateCommand>((testId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TestCaseResponse>(value));

            return resp;
        }

        [HttpDelete("{testId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TestCaseMessageResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<TestCaseMessageResponse>> TestCaseDelete([FromRoute] Guid testId, TestCaseDeleteRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TestCaseDeleteCommand>((testId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TestCaseMessageResponse>(value));

            return resp;
        }
    }
}
