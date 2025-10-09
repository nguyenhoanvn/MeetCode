using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTimeOffset ExpiresAt { get; set; } = default!;

    public DateTimeOffset CreatedAt { get; set; } = default!;

    public bool IsRevoked { get; set; } = false;

    public virtual User User { get; set; } = null!;

    public bool IsValid()
    {
        return !IsRevoked && ExpiresAt > DateTimeOffset.UtcNow;
    }
}
