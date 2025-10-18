using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Auth;

namespace MeetCode.Application.Queries.QueryEntities.Auth
{
    public sealed record LoginUserQuery(string Email, string Password) : IRequest<Result<LoginUserQueryResult>>;
}
