using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class GoalRepository : IGoalRepository
{
    private readonly PlannerDbContext context;

    public GoalRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.Goal> GetByIdAsync(Guid id)
    {
        return await context.Goals.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.Goal>> GetAllAsync()
    {
        return await context.Goals.ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.Goal goal)
    {
        await context.Goals.AddAsync(goal);
    }

    public async Task UpdateAsync(Domain.Entities.Goal goal)
    {
        context.Goals.Update(goal);
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.Goal goal = await context.Goals.FindAsync(id);
        if (goal != null)
        {
            context.Goals.Remove(goal);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}