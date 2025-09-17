using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class UserStat
{
    public Guid UserId { get; set; }

    public int TotalSolved { get; set; }

    public double TotalScore { get; set; }

    public int StreakDays { get; set; }

    public DateTimeOffset? LastSubmissionAt { get; set; }

    public int Rating { get; set; }

    public virtual User User { get; set; } = null!;
}
