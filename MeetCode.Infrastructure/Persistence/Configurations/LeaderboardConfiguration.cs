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
    public class LeaderboardConfiguration : IEntityTypeConfiguration<Leaderboard>
    {
        public void Configure(EntityTypeBuilder<Leaderboard> b)
        {
            b.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A006355A83CA");

            b.Property(e => e.LeaderboardId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.Period).HasMaxLength(20);
            b.Property(e => e.SnapshotAt).HasDefaultValueSql("(sysutcdatetime())");
        }
    }
}
