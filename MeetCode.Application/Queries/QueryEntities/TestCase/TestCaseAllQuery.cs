using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.TestCase;

namespace MeetCode.Application.Queries.QueryEntities.TestCase
{
    public sealed record TestCaseAllQuery() : IRequest<Result<TestCaseAllQueryResult>>;
}
