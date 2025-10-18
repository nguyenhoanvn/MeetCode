using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Problem;
using MeetCode.Application.Commands.CommandResults.Problem;

namespace MeetCode.Application.Queries.QueryEntities.Problem
{
    public sealed record ProblemReadQuery(string ProblemSlug) : IRequest<Result<ProblemReadQueryResult>>;

}
