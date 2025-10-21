using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using MeetCode.Server.DTOs.Request.Tag;
using MeetCode.Server.DTOs.Response.Tag;
using MeetCode.Server.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;

namespace MeetCode.Server.Controllers
{
    [Route("tag")]
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

        [HttpPost("create")]
        [ProducesResponseType(typeof(TagAddCommandResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateTag([FromBody] TagAddRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TagAddCommand>(request);

            var result = await _mediator.Send(cmd, ct);

            var resp = _mapper.Map<TagAddResponse>(result.Value);

            return CreatedAtAction(nameof(CreateTag), new { Id = resp.TagId }, resp);
        }

        [HttpGet]
        [ProducesResponseType(typeof(TagAllResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TagList(CancellationToken ct)
        {
            var request = new TagAllRequest();

            var cmd = _mapper.Map<TagAllQuery>(request);

            var result = await _mediator.Send(cmd, ct);

            var resp = _mapper.Map<TagAllResponse>(result.Value);

            return Ok(resp);
        }

        [HttpGet("{name}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(TagReadResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
        public async Task<IActionResult> TagRead([FromRoute] string name, CancellationToken ct)
        {
            var request = new TagReadRequest();
            var cmd = _mapper.Map<TagReadQuery>(request, opt =>
            opt.Items["Name"] = name);

            var result = await _mediator.Send(cmd, ct);

            var resp = _mapper.Map<TagReadResponse>(result.Value);
            return Ok(resp); 
        }

        [HttpPost("update/{tagId}")]
        [ProducesResponseType(typeof(TagUpdateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> TagUpdate([FromRoute] Guid tagId, [FromBody] TagUpdateRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TagUpdateCommand>(request, opt =>
                opt.Items["TagId"] = tagId);

            var result = await _mediator.Send(cmd, ct);

            var resp = _mapper.Map<TagUpdateCommandResult>(result.Value);

            return Ok(resp);
        }

        [HttpPost("delete/{tagId}")]
        [ProducesResponseType(typeof(TagDeleteResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> TagDelete([FromRoute] Guid tagId, [FromBody] TagDeleteRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<TagDeleteCommand>(request, opt =>
                opt.Items["TagId"] = tagId);

            var result = await _mediator.Send(cmd, ct);

            var resp = _mapper.Map<TagDeleteResponse>(result.Value);

            return Ok(resp);
        }
    }
}
