using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.DTOs.Request.Tag;
using MeetCode.Application.DTOs.Response.Tag;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public TagController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status201Created)]
        [ExpectedFailures(ResultStatus.Forbidden, ResultStatus.Conflict, ResultStatus.Error)]
        public async Task<Result<TagResponse>> TagCreate([FromBody] TagAddRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TagAddCommand>(request);

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TagResponse>(value));

            return resp;
        }

        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagAllResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<TagAllResponse>> TagList(CancellationToken ct)
        {
            var request = new TagAllRequest();

            var cmd = _mapper.Map<TagAllQuery>(request);

            var result = await _mediator.Send(cmd, ct);

            var resp = _mapper.Map<TagAllResponse>(result.Value);

            return resp;
        }

        [HttpGet("{tagId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
        public async Task<Result<TagResponse>> TagRead([FromRoute] Guid tagId, CancellationToken ct)
        {
            var request = new TagReadRequest();
            var cmd = _mapper.Map<TagReadQuery>((tagId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TagResponse>(value));

            return resp;
        }

        [HttpPut("{tagId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Conflict, ResultStatus.Forbidden, ResultStatus.Error)]

        public async Task<Result<TagResponse>> TagUpdate([FromRoute] Guid tagId, [FromBody] TagUpdateRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TagUpdateCommand>((tagId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TagResponse>(value));

            return resp;
        }

        [HttpDelete("{tagId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error, ResultStatus.Forbidden)]
        public async Task<Result<TagMessageResponse>> TagDelete([FromRoute] Guid tagId, [FromBody] TagDeleteRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TagDeleteCommand>((tagId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<TagMessageResponse>(value));

            return resp;
        }
    }
}
