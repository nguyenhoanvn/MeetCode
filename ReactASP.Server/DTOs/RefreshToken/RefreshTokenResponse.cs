using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.DTOs.RefreshToken
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = default!;
        public string NewRefreshToken { get; set; } = default!;
    }
}
