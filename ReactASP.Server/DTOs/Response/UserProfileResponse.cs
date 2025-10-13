using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Server.DTOs.Response
{
    public class UserProfileResponse
    {
        // Basic info
        public Guid UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public DateTime CreateAt { get; set; } = default!;
        public DateTime? LastLoginAt { get; set; } = default!;

        // Stats info
        public int TotalSolved { get; set; } = default!;
        public double TotalScore { get; set; } = default!;
        public int StreakDays { get; set; } = default!;

    }
}
