using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryResults.Auth
{
    public sealed record LoginUserQueryResult(
        string AccessToken,
        string RefreshToken, 
        string DisplayName,
        string Role,
        bool IsSuccessful,
        string Message);
}
