using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.Profile
{
    public sealed record ProfileUserResult(
        Guid UserId,
        string Email,
        string DisplayName,
        string Role,
        DateTime CreateAt,
        DateTime? LastLoginAt,
        int TotalSolved,
        double TotalScore,
        int StreakDays
        );
}
