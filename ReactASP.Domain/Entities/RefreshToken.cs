using System;
using System.Collections.Generic;

namespace ReactASP.Domain.Entities;

public partial class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTimeOffset ExpiresAt { get; set; } = DateTimeOffset.UtcNow.AddDays(10);

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsRevoked { get; set; } = false;

    public virtual User User { get; set; } = null!;

}
