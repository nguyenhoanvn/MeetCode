using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Profile;

namespace MeetCode.Application.Queries.QueryEntities.Profile
{
    public sealed record ProfileUserQuery(
        Guid UserId)
        : IRequest<Result<ProfileUserQueryResult>>;
}
