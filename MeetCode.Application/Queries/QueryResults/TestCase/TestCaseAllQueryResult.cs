using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Queries.QueryResults.TestCase
{
    public sealed record TestCaseAllQueryResult(IEnumerable<MeetCode.Domain.Entities.TestCase> TestCaseList);
}
