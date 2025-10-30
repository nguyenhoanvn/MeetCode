using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTimeOffset? LastLoginAt { get; set; }

    public string? AuthProvider { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();

    public virtual UserStat? UserStat { get; set; }
    public override string ToString()
    {
        return this.ToGenericString();
    }
}
