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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(t => t.IsCompleted)
                  .IsRequired();
            entity.Property(t => t.DueDate)
                  .IsRequired();
            entity.Property(t => t.CreatedAt)
                  .IsRequired();
        });
    }
}