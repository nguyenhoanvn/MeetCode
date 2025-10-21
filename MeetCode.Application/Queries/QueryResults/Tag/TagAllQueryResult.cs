using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Queries.QueryResults.Tag
{
    public sealed record TagAllQueryResult(IEnumerable<ProblemTag> TagList);
}
