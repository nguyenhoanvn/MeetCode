using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using ReactASP.Application.Interfaces;
using ReactASP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ReactASP.Infrastructure.Persistence;

public partial class AppDbContext : DbContext, IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<ProblemTag> ProblemTags { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<TestCase> TestCases { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBadge> UserBadges { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<UserStat> UserStats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DB"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditLog__A17F239826C8386F");

            entity.HasIndex(e => new { e.Entity, e.Timestamp }, "IX_AuditLogs_Entity_Timestamp").IsDescending(false, true);

            entity.Property(e => e.AuditId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Entity).HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.ActorUser).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ActorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditLogs_Users");
        });*/

        /*modelBuilder.Entity<Badge>(entity =>
        {
            entity.HasKey(e => e.BadgeId).HasName("PK__Badges__1918235CD5A25AF3");

            entity.HasIndex(e => e.Name, "UQ__Badges__737584F64C4652C2").IsUnique();

            entity.Property(e => e.BadgeId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Description).HasDefaultValue("Very long badge description");
            entity.Property(e => e.IconUrl).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });*/

        /*modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LangId).HasName("PK__Language__A5F312DEEE70E068");

            entity.Property(e => e.LangId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CompileCommand).HasMaxLength(500);
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.RunCommand).HasMaxLength(500);
            entity.Property(e => e.RuntimeImage).HasMaxLength(200);
            entity.Property(e => e.Version).HasMaxLength(50);
        });*/

        /*modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A006355A83CA");

            entity.Property(e => e.LeaderboardId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Period).HasMaxLength(20);
            entity.Property(e => e.SnapshotAt).HasDefaultValueSql("(sysutcdatetime())");
        });*/

        /*modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.ProblemId).HasName("PK__Problems__5CED528AEF752877");

            entity.HasIndex(e => new { e.Difficulty, e.CreatedAt }, "IX_Problems_Difficulty_CreatedAt").IsDescending(false, true);

            entity.HasIndex(e => e.Slug, "IX_Problems_Slug");

            entity.HasIndex(e => e.Slug, "UQ__Problems__BC7B5FB67ED9FA0C").IsUnique();

            entity.Property(e => e.ProblemId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AcceptanceRate).HasComputedColumnSql("(CONVERT([float],[ScoreAcceptedCount])/nullif([TotalSubmissionCount],(0)))", true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Difficulty).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Slug).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Problems)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Problems_Users");

            entity.HasMany(d => d.Tags).WithMany(p => p.Problems)
                .UsingEntity<Dictionary<string, object>>(
                    "ProblemTagMap",
                    r => r.HasOne<ProblemTag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_ProblemTagMap_Tags"),
                    l => l.HasOne<Problem>().WithMany()
                        .HasForeignKey("ProblemId")
                        .HasConstraintName("FK_ProblemTagMap_Problems"),
                    j =>
                    {
                        j.HasKey("ProblemId", "TagId").HasName("PK__ProblemT__8ABA9D101D97518E");
                        j.ToTable("ProblemTagMap");
                    });
        });*/

        /*modelBuilder.Entity<ProblemTag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__ProblemT__657CF9AC80AA5392");

            entity.HasIndex(e => e.Name, "UQ__ProblemT__737584F6291B9DBE").IsUnique();

            entity.Property(e => e.TagId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Name).HasMaxLength(100);
        });*/

        /*modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("PK__Submissi__449EE125307DD79B");

            entity.HasIndex(e => new { e.ProblemId, e.UserId, e.FinishedAt }, "IX_Submissions_Problem_User_FinishedAt").IsDescending(false, false, true);

            entity.HasIndex(e => e.Verdict, "IX_Submissions_Verdict");

            entity.Property(e => e.SubmissionId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.EnqueuedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ErrorLogRef).HasMaxLength(500);
            entity.Property(e => e.Verdict).HasMaxLength(30);

            entity.HasOne(d => d.Lang).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Languages");

            entity.HasOne(d => d.Problem).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.ProblemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Problems");

            entity.HasOne(d => d.User).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Submissions_Users");
        });*/

        /*modelBuilder.Entity<TestCase>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestCase__8CC331604C2812DC");

            entity.Property(e => e.TestId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Visibility).HasMaxLength(20);
            entity.Property(e => e.Weight).HasDefaultValue(1);

            entity.HasOne(d => d.Problem).WithMany(p => p.TestCases)
                .HasForeignKey(d => d.ProblemId)
                .HasConstraintName("FK_TestCases_Problems");
        });*/

        /*modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF99D00AF");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105340E9CB287").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.AuthProvider).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
        });*/

        /*modelBuilder.Entity<UserBadge>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BadgeId }).HasName("PK__UserBadg__C6194E7972301F30");

            entity.Property(e => e.AwardedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Badge).WithMany(p => p.UserBadges)
                .HasForeignKey(d => d.BadgeId)
                .HasConstraintName("FK_UserBadges_Badges");

            entity.HasOne(d => d.User).WithMany(p => p.UserBadges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserBadges_Users");
        });*/

        /*modelBuilder.Entity<UserStat>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserStat__1788CC4CB7F924D2");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Rating).HasDefaultValue(1200);

            entity.HasOne(d => d.User).WithOne(p => p.UserStat)
                .HasForeignKey<UserStat>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserStats_Users");
        });*/

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await base.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        if (_currentTransaction is not null) return;

        _currentTransaction = await Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("No active transaction to commit");
        }

        try
        {
            await SaveChangesAsync(ct);
            await _currentTransaction.CommitAsync(ct);
        } catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        } finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct)
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.RollbackAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
