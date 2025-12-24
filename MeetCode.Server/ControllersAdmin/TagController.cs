using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.DTOs.Request.Tag;
using MeetCode.Application.DTOs.Response.Tag;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using MeetCode.Application.Queries.QueryResults.Tag;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("admin/tags")]
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
        [ProducesResponseType(typeof(TagAllQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<TagAllQueryResult>> TagList(CancellationToken ct)
        {
            var cmd = new TagAllQuery();

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("search")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagSearchQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<TagSearchQueryResult>> TagSearch([FromQuery] string name, CancellationToken ct)
        {
            var cmd = new TagSearchQuery(name);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("{tagId:guid}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagReadQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Error)]
        public async Task<Result<TagReadQueryResult>> TagRead([FromRoute] Guid tagId, CancellationToken ct)
        {
            var cmd = new TagReadByIdQuery(tagId);

            var result = await _mediator.Send(cmd, ct);

            return result;
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
