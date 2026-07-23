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
    public DbSet<YearlyPlan> YearlyPlans => Set<YearlyPlan>();
    public DbSet<MonthlyPlan> MonthlyPlans => Set<MonthlyPlan>();
    public DbSet<WeeklyPlan> WeeklyPlans => Set<WeeklyPlan>();
    public DbSet<DailyPlan> DailyPlans => Set<DailyPlan>();

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
            entity.HasMany(g => g.YearlyPlans).WithOne(sg => sg.Goal).HasForeignKey(sg => sg.GoalId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<YearlyPlan>(entity =>
        {
            entity.ToTable("YearlyPlans");
            entity.HasKey(sg => sg.Id);
            entity.Property(sg => sg.Title).IsRequired().HasMaxLength(200);
            entity.Property(sg => sg.CreatedAt).IsRequired();
            entity.Property(sg => sg.DueDate).IsRequired();
            entity.Property(sg => sg.IsCompleted).IsRequired();
            entity.HasMany(y => y.MonthlyPlans).WithOne(m => m.YearlyPlan).HasForeignKey(m => m.YearlyPlanId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MonthlyPlan>(entity =>
        {
            entity.ToTable("MonthlyPlans");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(200);
            entity.HasMany(m => m.WeeklyPlans).WithOne(w => w.MonthlyPlan).HasForeignKey(w => w.MonthlyPlanId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WeeklyPlan>(entity =>
        {
            entity.ToTable("WeeklyPlans");
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Title).IsRequired().HasMaxLength(200);
            entity.HasMany(w => w.DailyPlans).WithOne(d => d.WeeklyPlan).HasForeignKey(d => d.WeeklyPlanId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DailyPlan>(entity =>
        {
            entity.ToTable("DailyPlans");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Title).IsRequired().HasMaxLength(200);
        });
    }
}
