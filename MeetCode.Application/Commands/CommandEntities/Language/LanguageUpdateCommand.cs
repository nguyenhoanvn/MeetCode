using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Language;

namespace MeetCode.Application.Commands.CommandEntities.Language
{
    public sealed record LanguageUpdateCommand(
        Guid LangId,
        string? Name,
        string? Version,
        string? FileExtension,
        string? CompileImage,
        string? RuntimeImage,
        string? CompileCommand,
        string? RunCommand
        ) : IRequest<Result<LanguageUpdateCommandResult>>;
}
