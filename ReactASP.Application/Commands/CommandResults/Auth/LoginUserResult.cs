using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.CommandResults.Auth
{
    public sealed record LoginUserResult(string AccessToken, string RefreshToken, string DisplayName, string Role);
}
