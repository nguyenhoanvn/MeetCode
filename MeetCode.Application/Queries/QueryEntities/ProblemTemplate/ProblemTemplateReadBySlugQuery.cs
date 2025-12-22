using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.ProblemTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.ProblemTemplate
{
    public sealed record ProblemTemplateReadBySlugQuery(string ProblemSlug) : IRequest<Result<ProblemTemplateReadQueryResult>>;
}
