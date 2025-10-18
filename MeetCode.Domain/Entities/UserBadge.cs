using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class UserBadge
{
    public Guid UserId { get; set; }

    public Guid BadgeId { get; set; }

    public DateTimeOffset AwardedAt { get; set; }

    public virtual Badge Badge { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
