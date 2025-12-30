using MeetCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Persistence.Configurations
{
    public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
    {
        public void Configure(EntityTypeBuilder<Problem> b)
        {
            b.HasKey(e => e.ProblemId).HasName("PK__Problems__5CED528AEF752877");

            b.HasIndex(e => new { e.Difficulty, e.CreatedAt }, "IX_Problems_Difficulty_CreatedAt").IsDescending(false, true);

            b.HasIndex(e => e.Slug, "IX_Problems_Slug");

            b.HasIndex(e => e.Slug, "UQ__Problems__BC7B5FB67ED9FA0C").IsUnique();

            b.Property(e => e.ProblemId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.AcceptanceRate)
                 .HasComputedColumnSql(
                     "(CONVERT([float],[ScoreAcceptedCount]) / NULLIF([TotalSubmissionCount], 0)) * 100.0",
                     stored: true
                 );
            b.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            b.Property(e => e.Difficulty).HasMaxLength(20);
            b.Property(e => e.IsActive).HasDefaultValue(true);
            b.Property(e => e.Slug).HasMaxLength(200);
            b.Property(e => e.Title).HasMaxLength(255);

            b.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Problems)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Problems_Users");

            b.HasMany(d => d.Tags).WithMany(p => p.Problems)
                .UsingEntity<Dictionary<string, object>>(
                    "ProblemTagMap",
                    r => r.HasOne<ProblemTag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_ProblemTagMap_Tags"),
                    l => l.HasOne<Problem>().WithMany()
                        .HasForeignKey("ProblemId")
                        .HasConstraintName("FK_ProblemTagMap_Problems"),
                    j =>
                    {
                        j.HasKey("ProblemId", "TagId").HasName("PK__ProblemT__8ABA9D101D97518E");
                        j.ToTable("ProblemTagMap");
                    });
        }
    }
}
