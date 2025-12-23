using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Problem
{
    public sealed record ProblemToggleCommand(
        Guid ProblemId
        ) : IRequest<Result<ProblemToggleCommandResult>>;
}
