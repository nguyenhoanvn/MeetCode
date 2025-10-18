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
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> b)
        {
            b.HasKey(e => e.LangId).HasName("PK__Language__A5F312DEEE70E068");

            b.Property(e => e.LangId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.CompileCommand).HasMaxLength(500);
            b.Property(e => e.IsEnabled).HasDefaultValue(true);
            b.Property(e => e.Name).HasMaxLength(100);
            b.Property(e => e.RunCommand).HasMaxLength(500);
            b.Property(e => e.RuntimeImage).HasMaxLength(200);
            b.Property(e => e.Version).HasMaxLength(50);
        }
    }
}
