using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Problem;

namespace MeetCode.Application.Commands.CommandEntities.Problem
{
    public sealed record ProblemUpdateCommand(
        Guid ProblemId,
        string NewStatementMd,
        string NewDifficulty,
        IEnumerable<Guid> TagIds
        ) : IRequest<Result<ProblemUpdateCommandResult>>;
}
