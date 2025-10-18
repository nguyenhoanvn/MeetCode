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
    public class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
    {
        public void Configure(EntityTypeBuilder<TestCase> b)
        {
            b.HasKey(e => e.TestId).HasName("PK__TestCase__8CC331604C2812DC");

            b.Property(e => e.TestId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.Visibility).HasMaxLength(20);
            b.Property(e => e.Weight).HasDefaultValue(1);

            b.HasOne(d => d.Problem).WithMany(p => p.TestCases)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("FK_TestCases_Problems");
        }
    }
}
