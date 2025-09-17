using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactASP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Infrastructure.Persistence.Configurations
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> b)
        {
            b.HasKey(e => e.SubmissionId).HasName("PK__Submissi__449EE125307DD79B");

            b.HasIndex(e => new { e.ProblemId, e.UserId, e.FinishedAt }, "IX_Submissions_Problem_User_FinishedAt").IsDescending(false, false, true);

            b.HasIndex(e => e.Verdict, "IX_Submissions_Verdict");

            b.Property(e => e.SubmissionId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.EnqueuedAt).HasDefaultValueSql("(sysutcdatetime())");
            b.Property(e => e.ErrorLogRef).HasMaxLength(500);
            b.Property(e => e.Verdict).HasMaxLength(30);

            b.HasOne(d => d.Lang).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Languages");

            b.HasOne(d => d.Problem).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.ProblemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Problems");

            b.HasOne(d => d.User).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Users");
        }
    }
}
