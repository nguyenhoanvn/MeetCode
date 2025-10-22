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
    public sealed record ProblemDeleteCommand(Guid ProblemId) : IRequest<Result<ProblemDeleteCommandResult>>;
}
