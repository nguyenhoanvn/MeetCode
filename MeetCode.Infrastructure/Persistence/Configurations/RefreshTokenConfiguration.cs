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
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> b)
        {
            b.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07B84C2AF1");

            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.TokenHash).HasMaxLength(200);

            b.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__17036CC0");

            b.Property(r => r.CreatedAt).HasColumnType("datetimeoffset");
            b.Property(r => r.ExpiresAt).HasColumnType("datetimeoffset");
        }
    }
}
