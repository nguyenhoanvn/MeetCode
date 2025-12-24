using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults.Tag
{
    public sealed record TagSearchQueryResult(List<MeetCode.Domain.Entities.ProblemTag> TagList);
}
