using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MeetCode.Domain.Entities;

public partial class Problem
{
    public Guid ProblemId { get; set; }

    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string StatementMd { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public int TimeLimitMs { get; set; } = 1;

    public int MemoryLimitMb { get; set; } = 1;

    public Guid CreatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public int TotalSubmissionCount { get; set; } = 0;

    public int ScoreAcceptedCount { get; set; } = 0;

    public double? AcceptanceRate { get; set; }

    public bool IsActive { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();

    public virtual ICollection<ProblemTag> Tags { get; set; } = new List<ProblemTag>();
    public virtual ICollection<ProblemTemplate> ProblemTemplates { get; set; } = new List<ProblemTemplate>();

    public override string ToString()
    {
        return this.ToGenericString();
    }

    public void GenerateSlug()
    {
        Slug = Regex.Replace(Title.ToLowerInvariant().Trim(), @"\s+", "-").ToLowerInvariant();
    }

    public void UpdateSubmission(string submissionVerdict)
    {
        TotalSubmissionCount++;
        if (submissionVerdict == "accepted")
        {
            ScoreAcceptedCount++;
        }
        AcceptanceRate = ScoreAcceptedCount / TotalSubmissionCount;
    }

    public void ToggleStatus()
    {
        IsActive = !IsActive;
    }

    public void UpdateBasic(string statementMd, string difficulty)
    {
        StatementMd = statementMd;
        Difficulty = difficulty;
    }

    public void UpdateTags(IEnumerable<ProblemTag> tagList)
    {
        Tags.Clear();
        foreach(var tag in tagList)
        {
            Tags.Add(tag);
        }
    }
}
