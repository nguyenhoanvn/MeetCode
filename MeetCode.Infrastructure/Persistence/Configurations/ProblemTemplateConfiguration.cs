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
    public class ProblemTemplateConfiguration : IEntityTypeConfiguration<ProblemTemplate>
    {
        public void Configure(EntityTypeBuilder<ProblemTemplate> b)
        {
            b.HasKey(e => e.TemplateId).HasName("PK__ProblemT__F87ADD27AC0D808E");

            b.Property(e => e.TemplateId).HasDefaultValueSql("(newsequentialid())");
            b.Property(e => e.IsEnabled).HasDefaultValue(true);

            b.HasOne(e => e.Problem).WithMany(p => p.ProblemTemplates)
                .HasForeignKey(e => e.ProblemId)
                .HasConstraintName("FK_ProblemTemplates_Problems")
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.Language).WithMany(l => l.ProblemTemplates)
                .HasForeignKey(e => e.LangId)
                .HasConstraintName("FK_ProblemTemplates_Languages")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
