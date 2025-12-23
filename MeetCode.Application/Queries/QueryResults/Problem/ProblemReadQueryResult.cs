using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults.Problem
{
    public sealed record ProblemReadQueryResult(
        MeetCode.Domain.Entities.Problem Problem
        );
}
