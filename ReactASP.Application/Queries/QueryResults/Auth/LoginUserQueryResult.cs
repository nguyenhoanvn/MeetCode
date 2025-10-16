using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Queries.QueryResults.Auth
{
    public sealed record LoginUserQueryResult(string AccessToken, string RefreshToken, string DisplayName, string Role);
}
