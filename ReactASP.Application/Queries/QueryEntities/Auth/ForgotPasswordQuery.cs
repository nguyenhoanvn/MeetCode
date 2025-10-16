using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Queries.QueryResults.Auth;

namespace ReactASP.Application.Queries.QueryEntities.Auth
{
    public sealed record ForgotPasswordQuery(string Email) : IRequest<Result<ForgotPasswordQueryResult>>;
}
