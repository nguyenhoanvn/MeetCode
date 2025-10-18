using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Problem;

namespace MeetCode.Application.Queries.QueryEntities.Problem
{
    public sealed record ProblemAllQuery() : IRequest<Result<ProblemAllQueryResult>>;
}
