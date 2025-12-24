using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.Tag
{
    public sealed record TagSearchQuery(
        string TagName
        ) : IRequest<Result<TagSearchQueryResult>>;
}
