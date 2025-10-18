using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task UpdateLoginTime(Guid userId, CancellationToken ct);
        Task<string> GetEmailFromOtpAsync(string otp);
    }
}
