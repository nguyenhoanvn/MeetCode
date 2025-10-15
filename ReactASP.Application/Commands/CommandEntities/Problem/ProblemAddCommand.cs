using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Commands.CommandResults.Problem;

namespace ReactASP.Application.Commands.CommandEntities.Problem
{
    public sealed record ProblemAddCommand(
        string Title,
        string StatementMd,
        string Difficulty,
        int TimeLimitMs,
        int MemoryLimitMb
        ) : IRequest<Result<ProblemAddResult>>;
}
