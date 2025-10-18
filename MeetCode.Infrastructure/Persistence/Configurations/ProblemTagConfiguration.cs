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
    public class ProblemTagConfiguration : IEntityTypeConfiguration<ProblemTag>
    {
        public void Configure(EntityTypeBuilder<ProblemTag> b)
        {
            b.HasKey(e => e.TagId).HasName("PK__ProblemT__657CF9AC80AA5392");

            b.HasIndex(e => e.Name, "UQ__ProblemT__737584F6291B9DBE").IsUnique();

            b.Property(e => e.TagId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.Name).HasMaxLength(100);
        }
    }
}
