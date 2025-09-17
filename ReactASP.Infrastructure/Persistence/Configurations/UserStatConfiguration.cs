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
    public class UserStatConfiguration : IEntityTypeConfiguration<UserStat>
    {
        public void Configure(EntityTypeBuilder<UserStat> b)
        {
            b.HasKey(e => e.UserId).HasName("PK__UserStat__1788CC4CB7F924D2");

            b.Property(e => e.UserId).ValueGeneratedNever();
            b.Property(e => e.Rating).HasDefaultValue(1200);

            b.HasOne(d => d.User).WithOne(p => p.UserStat)
                .HasForeignKey<UserStat>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserStats_Users");
        }
    }
}
