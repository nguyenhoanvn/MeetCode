using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using MeetCode.Application.DTOs.Request.ProblemTemplate;
using MeetCode.Application.DTOs.Response.ProblemTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Server.Controllers
{
    [Route("templates")]
    [ApiController]
    public class ProblemTemplateController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        
        public ProblemTemplateController(
            ISender mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("{problemId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemTemplateResponse), StatusCodes.Status201Created)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemTemplateResponse>> TemplateAdd(Guid problemId, ProblemTemplateAddRequest request, CancellationToken ct)
        {
            var cmd = _mapper.Map<ProblemTemplateAddCommand>((problemId, request));

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<ProblemTemplateResponse>(value));

            return resp;
        }
    }
}
