using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Queries.QueryResults.Problem;

namespace ReactASP.Application.Queries.QueryEntities.Problem
{
    public sealed record ProblemAllQuery() : IRequest<Result<ProblemAllQueryResult>>;
}
