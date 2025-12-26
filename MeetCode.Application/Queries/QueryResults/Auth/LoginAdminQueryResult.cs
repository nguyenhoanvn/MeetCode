using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults.Auth
{
    public sealed record LoginAdminQueryResult(
        MeetCode.Domain.Entities.User User, 
        string AccessToken,
        string RefreshToken
        );
}
