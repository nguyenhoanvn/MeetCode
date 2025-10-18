using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Server.DTOs.Response.Auth
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken
        );
}
