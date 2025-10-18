using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class AuditLog
{
    public Guid AuditId { get; set; }

    public Guid ActorUserId { get; set; }

    public string Action { get; set; } = null!;

    public string Entity { get; set; } = null!;

    public Guid? EntityId { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public string? MetadataJson { get; set; }

    public virtual User ActorUser { get; set; } = null!;
}
