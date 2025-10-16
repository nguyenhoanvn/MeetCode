using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Queries.QueryResults.Problem
{
    public sealed record ProblemAllQueryResult(IEnumerable<ReactASP.Domain.Entities.Problem> ProblemList);
}
