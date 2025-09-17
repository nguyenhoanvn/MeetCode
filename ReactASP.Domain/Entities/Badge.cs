using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class Badge
{
    public Guid BadgeId { get; set; }

    public string Name { get; set; } = null!;

    public string RuleJson { get; set; } = null!;

    public string? IconUrl { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}
