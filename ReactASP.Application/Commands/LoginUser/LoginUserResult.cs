using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.LoginUser
{
    public sealed record LoginUserResult(string accessToken, string refreshToken, string displayName, string role);
}
