using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Server.DTOs.Response
{
    public sealed record UserProfileResponse(
        Guid UserId,
        string Email,
        string DisplayName,
        string Role,
        int TotalSolved,
        double TotalScore,
        int StreakDays
        );
}
