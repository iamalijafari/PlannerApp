using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;

namespace Planner.Infrastructure.Persistence;

public class PlannerDbContext : DbContext
{
    public PlannerDbContext(DbContextOptions<PlannerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<YearlyGoal> YearlyGoals => Set<YearlyGoal>();
    public DbSet<MonthlyGoal> MonthlyGoals => Set<MonthlyGoal>();
    public DbSet<WeeklyGoal> WeeklyGoals => Set<WeeklyGoal>();
    public DbSet<DailyGoal> DailyGoals => Set<DailyGoal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Title).IsRequired().HasMaxLength(200);
            entity.Property(g => g.CreatedAt).IsRequired();
            entity.Property(g => g.DueDate).IsRequired();
            entity.Property(g => g.IsCompleted).IsRequired();
            entity.HasMany(g => g.YearlyGoals).WithOne(sg => sg.Goal).HasForeignKey(sg => sg.GoalId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<YearlyGoal>(entity =>
        {
            entity.HasKey(sg => sg.Id);
            entity.Property(sg => sg.Title).IsRequired().HasMaxLength(200);
            entity.Property(sg => sg.CreatedAt).IsRequired();
            entity.Property(sg => sg.DueDate).IsRequired();
            entity.Property(sg => sg.IsCompleted).IsRequired();
            entity.HasMany(y => y.MonthlyGoals).WithOne(m => m.YearlyGoal).HasForeignKey(m => m.YearlyGoalId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MonthlyGoal>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
            entity.HasMany(m => m.WeeklyGoals).WithOne(w => w.MonthlyGoal).HasForeignKey(w => w.MonthlyGoalId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WeeklyGoal>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Title).IsRequired().HasMaxLength(200);
            entity.HasMany(w => w.DailyGoals).WithOne(d => d.WeeklyGoal).HasForeignKey(d => d.WeeklyGoalId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DailyGoal>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Title).IsRequired().HasMaxLength(200);
        });
    }
}