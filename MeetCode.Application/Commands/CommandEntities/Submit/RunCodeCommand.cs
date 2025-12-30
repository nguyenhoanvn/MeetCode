using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Submit
{
    public sealed record RunCodeCommand(
        Guid UserId,
        string Code,
        Guid JobId,
        string LanguageName,
        Guid ProblemId,
        List<Guid> TestCaseIds
        ) : IRequest<Result<RunCodeCommandResult>>;
}
