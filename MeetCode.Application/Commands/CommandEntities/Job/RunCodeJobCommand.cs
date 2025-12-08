using Ardalis.Result;
using MediatR;
using MeetCode.Application.DTOs.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Job
{
    public sealed record RunCodeJobCommand(
        string Code,
        string LanguageName,
        Guid ProblemId,
        List<Guid> TestCaseIds
        ) : IRequest<Result<EnqueueResult<RunCodeJobCommand>>>;
}
