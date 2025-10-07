using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.ProblemAdd
{
    public sealed record ProblemAddResult(
        Guid ProblemId,
        string Slug,
        string Title,
        string StatementMd,
        string Difficulty,
        int TimeLimitMs,
        int MemoryLimitMb,
        DateTimeOffset CreatedAt,
        Guid CreatedBy
        );
}
