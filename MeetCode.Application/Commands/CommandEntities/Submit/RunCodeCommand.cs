using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Submit
{
    public sealed record RunCodeCommand(
        string Code,
        Guid LanguageId,
        Guid ProblemId,
        List<Guid> TestCaseIds
        ) : IRequest<Result<RunCodeCommandResult>>;
}
