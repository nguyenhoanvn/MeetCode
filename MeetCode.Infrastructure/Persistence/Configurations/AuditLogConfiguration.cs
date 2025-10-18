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
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> b)
        {
            b.HasKey(e => e.AuditId).HasName("PK__AuditLog__A17F239826C8386F");

            b.HasIndex(e => new { e.Entity, e.Timestamp }, "IX_AuditLogs_Entity_Timestamp").IsDescending(false, true);

            b.Property(e => e.AuditId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.Action).HasMaxLength(100);
            b.Property(e => e.Entity).HasMaxLength(100);
            b.Property(e => e.Timestamp).HasDefaultValueSql("(sysutcdatetime())");

            b.HasOne(d => d.ActorUser).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ActorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditLogs_Users");
        }
    }
}
