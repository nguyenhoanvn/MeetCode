using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults.Language
{
    public sealed record LanguageAllQueryResult(IEnumerable<MeetCode.Domain.Entities.Language> LanguageList);
}
