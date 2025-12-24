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
        });
    }
}