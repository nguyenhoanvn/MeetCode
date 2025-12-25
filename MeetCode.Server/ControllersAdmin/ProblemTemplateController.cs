using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using MeetCode.Application.Commands.CommandResults.ProblemTemplate;
using MeetCode.Application.DTOs.Request.ProblemTemplate;
using MeetCode.Application.DTOs.Response.ProblemTemplate;
using MeetCode.Application.Queries.QueryEntities.ProblemTemplate;
using MeetCode.Application.Queries.QueryResults.ProblemTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("admin/templates")]
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

        [HttpGet()]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemTemplateAllQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<ProblemTemplateAllQueryResult>> TemplateAll(CancellationToken ct)
        {
            var cmd = new ProblemTemplateAllQuery();

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("{templateId:guid}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemTemplateReadQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<ProblemTemplateReadQueryResult>> TemplateReadById([FromRoute] Guid templateId, CancellationToken ct)
        {
            var cmd = new ProblemTemplateReadByIdQuery(templateId);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpPost()]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemTemplateAddCommandResult), StatusCodes.Status201Created)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<ProblemTemplateAddCommandResult>> TemplateAdd([FromBody] ProblemTemplateAddRequest request, CancellationToken ct)
        {
            var cmd = new ProblemTemplateAddCommand(
                    request.ProblemId,
                    request.LangId,
                    request.MethodName,
                    request.ReturnType,
                    request.Parameters.Select(p => p.ToString()).ToArray(),
                    request.CompileCommand,
                    request.RunCommand
                );

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpPatch("{templateId}/toggle")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemTemplateToggleCommandResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<ProblemTemplateToggleCommandResult>> TemplateToggle(Guid templateId, CancellationToken ct)
        {
            var cmd = new ProblemTemplateToggleCommand(templateId);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet("{problemSlug}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(ProblemTemplateResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<ProblemTemplateResponse>> TemplateRead([FromRoute] string problemSlug, CancellationToken ct)
        {
            var cmd = new ProblemTemplateReadBySlugQuery(problemSlug);

            var result = await _mediator.Send(cmd, ct);

            var resp = result.Map(value => _mapper.Map<ProblemTemplateResponse>(value));

            return resp;
        }
    }
}
