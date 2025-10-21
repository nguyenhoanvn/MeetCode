using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Tag;

namespace MeetCode.Application.Queries.QueryEntities.Tag
{
    public sealed record TagAllQuery() : IRequest<Result<TagAllQueryResult>>;
}
