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
    public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> b)
        {
            b.HasKey(e => e.BadgeId).HasName("PK__Badges__1918235CD5A25AF3");

            b.HasIndex(e => e.Name, "UQ__Badges__737584F64C4652C2").IsUnique();

            b.Property(e => e.BadgeId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.Description).HasDefaultValue("Very long badge description");
            b.Property(e => e.IconUrl).HasMaxLength(500);
            b.Property(e => e.Name).HasMaxLength(100);
        }
    }
}
