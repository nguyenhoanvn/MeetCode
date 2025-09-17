using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class Leaderboard
{
    public Guid LeaderboardId { get; set; }

    public string Period { get; set; } = null!;

    public DateTimeOffset SnapshotAt { get; set; }

    public string PayloadJson { get; set; } = null!;
}
