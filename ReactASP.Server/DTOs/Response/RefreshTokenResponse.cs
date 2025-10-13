using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Server.DTOs.Response
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
