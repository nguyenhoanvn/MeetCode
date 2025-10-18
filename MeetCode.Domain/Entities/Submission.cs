using System;
using System.Collections.Generic;

namespace MeetCode.Domain.Entities;

public partial class Submission
{
    public Guid SubmissionId { get; set; }

    public Guid UserId { get; set; }

    public Guid ProblemId { get; set; }

    public Guid LangId { get; set; }

    public string Verdict { get; set; } = null!;

    public string SourceCode { get; set; } = null!;

    public DateTimeOffset EnqueuedAt { get; set; }

    public DateTimeOffset? StartedAt { get; set; }

    public DateTimeOffset? FinishedAt { get; set; }

    public int? ExecTimeMs { get; set; }

    public int? MemoryKb { get; set; }

    public double? Score { get; set; }

    public string? ErrorLogRef { get; set; }

    public int Retries { get; set; }

    public virtual Language Lang { get; set; } = null!;

    public virtual Problem Problem { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
