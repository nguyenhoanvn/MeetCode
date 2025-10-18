using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandResults
{
    public sealed record ProfileUserResult(
        Guid userId,
        string email,
        string displayName,
        string role,
        DateTime createAt,
        DateTime? lastLoginAt,
        int totalSolved,
        double totalScore,
        int streakDays
        );
}
