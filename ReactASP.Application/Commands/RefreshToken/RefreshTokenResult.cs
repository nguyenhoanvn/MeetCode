using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.RefreshToken
{
    public sealed record RefreshTokenResult(string AccessToken, string RefreshToken);
}
