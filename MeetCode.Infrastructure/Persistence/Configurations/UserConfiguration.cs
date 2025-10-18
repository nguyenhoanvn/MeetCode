using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> b)
        {
            b.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF99D00AF");

            b.HasIndex(e => e.Email, "IX_Users_Email");

            b.HasIndex(e => e.Email, "UQ__Users__A9D105340E9CB287").IsUnique();

            b.Property(e => e.UserId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.AuthProvider).HasMaxLength(50);
            b.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            b.Property(e => e.DisplayName).HasMaxLength(100);
            b.Property(e => e.Email).HasMaxLength(255);
            b.Property(e => e.IsActive).HasDefaultValue(true);
            b.Property(e => e.PasswordHash)
                .HasMaxLength(512)
                .IsUnicode(false);
            b.Property(e => e.Role).HasMaxLength(50);
        }
    }
}
