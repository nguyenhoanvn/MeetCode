using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Commands.CommandResults.Problem;
using ReactASP.Application.Queries.QueryResults.Problem;

namespace ReactASP.Application.Queries.QueryEntities.Problem
{
    public sealed record ProblemReadQuery(string ProblemSlug) : IRequest<Result<ProblemReadQueryResult>>;

}
