using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults.ProblemTemplate
{
    public sealed record ProblemTemplateReadQueryResult(
        MeetCode.Domain.Entities.ProblemTemplate ProblemTemplate,
        MeetCode.Domain.Entities.Language Language,
        MeetCode.Domain.Entities.Problem Problem);
}
