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
    public class UserBadgeConfiguration : IEntityTypeConfiguration<UserBadge>
    {
        public void Configure(EntityTypeBuilder<UserBadge> b)
        {
            b.HasKey(e => new { e.UserId, e.BadgeId }).HasName("PK__UserBadg__C6194E7972301F30");

            b.Property(e => e.AwardedAt).HasDefaultValueSql("(sysutcdatetime())");

            b.HasOne(d => d.Badge).WithMany(p => p.UserBadges)
                .HasForeignKey(d => d.BadgeId)
                .HasConstraintName("FK_UserBadges_Badges");

            b.HasOne(d => d.User).WithMany(p => p.UserBadges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserBadges_Users");
        }
    }
}
