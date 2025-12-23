using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.Problem
{
    public sealed record ProblemReadByIdQuery(
        Guid ProblemId
        ) : IRequest<Result<ProblemReadQueryResult>>;
}
