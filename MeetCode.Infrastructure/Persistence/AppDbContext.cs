using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MeetCode.Infrastructure.Persistence;

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
