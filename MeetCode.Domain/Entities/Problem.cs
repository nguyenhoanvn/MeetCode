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

        public int ScoreAcceptedCount { get; set; }

        public double? AcceptanceRate { get; set; }

        public bool IsActive { get; set; }

        public virtual User CreatedByNavigation { get; set; } = null!;

        public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

        public virtual ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();

        public virtual ICollection<ProblemTag> Tags { get; set; } = new List<ProblemTag>();

        public void GenerateSlug()
        {
        Slug = Regex.Replace(Title.ToLowerInvariant().Trim(), @"\s+", "-").ToLowerInvariant();
        }
}
