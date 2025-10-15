using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.CommandResults.Problem
{
    public sealed record ProblemAddResult(
        Guid problemId,
        string slug,
        string title,
        string statementMd,
        string difficulty,
        int timeLimitMs,
        int memoryLimitMb,
        DateTimeOffset createdAt,
        Guid createdBy
        );
}
