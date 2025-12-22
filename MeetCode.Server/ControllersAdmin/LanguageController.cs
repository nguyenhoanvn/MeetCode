
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using AutoMapper;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Commands.CommandResults.Language;
using MeetCode.Application.DTOs.Request.Language;
using MeetCode.Application.DTOs.Response.Language;
using MeetCode.Application.Queries.QueryEntities.Language;
using MeetCode.Application.Queries.QueryResults.Language;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Server.ControllersAdmin
{
    [Route("admin/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ISender _mediator;
        public LanguageController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{langId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(LanguageResponse), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.NotFound)]
        public async Task<Result<LanguageReadQueryResult>> LanguageRead([FromRoute] Guid langId, CancellationToken ct)
        {
            var cmd = new LanguageReadQuery(langId);

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(LanguageAllQueryResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error)]
        public async Task<Result<LanguageAllQueryResult>> LanguageList(CancellationToken ct)
        {
            var cmd = new LanguageAllQuery();

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpPatch("{langId}")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(LanguageUpdateCommandResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.NotFound)]
        public async Task<Result<LanguageUpdateCommandResult>> LanguageUpdate([FromRoute] Guid langId, [FromBody] LanguageUpdateRequest request, CancellationToken ct)
        {
            var cmd = new LanguageUpdateCommand(
                langId,
                request.Name,
                request.Version,
                request.FileExtension,
                request.CompileImage,
                request.RuntimeImage,
                request.CompileCommand,
                request.RunCommand
                );

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

        [HttpPatch("{langId}/toggle")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(LanguageStatusToggleCommandResult), StatusCodes.Status200OK)]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<Result<LanguageStatusToggleCommandResult>> LanguageToggle([FromRoute] Guid langId, CancellationToken ct)
        {
            var cmd = new LanguageStatusToggleCommand(
                langId
                );

            var result = await _mediator.Send(cmd, ct);

            return result;
        }

    }
}
