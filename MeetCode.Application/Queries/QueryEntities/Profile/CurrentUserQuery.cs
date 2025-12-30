using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.Profile
{
    public sealed record CurrentUserQuery(Guid UserId) : IRequest<Result<CurrentUserQueryResult>>;
}
