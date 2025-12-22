using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Language
{
    public sealed record LanguageStatusToggleCommand(
        Guid LangId
        ) : IRequest<Result<LanguageStatusToggleCommandResult>>;
}
