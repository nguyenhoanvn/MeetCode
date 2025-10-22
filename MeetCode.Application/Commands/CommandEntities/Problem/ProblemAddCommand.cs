using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Problem;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Commands.CommandEntities.Problem
{
    public sealed record ProblemAddCommand(
        string Title,
        string StatementMd,
        string Difficulty,
        int TimeLimitMs,
        int MemoryLimitMb,
        List<Guid> TagIds
        ) : IRequest<Result<ProblemAddCommandResult>>;
}
