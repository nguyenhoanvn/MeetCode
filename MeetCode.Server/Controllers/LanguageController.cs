using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Server.DTOs.Request.Language;
using MeetCode.Server.DTOs.Response.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Server.Controllers
{
    [Route("languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public LanguageController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPatch("{name}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(LanguageResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.NotFound)]
        public async Task<Result<LanguageResponse>> LanguageUpdate([FromRoute] string name, [FromBody] LanguageUpdateRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<LanguageUpdateCommand>((name, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<LanguageResponse>(value));

            return resp;
        }

    }
}
