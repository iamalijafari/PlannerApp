using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class DailyGoalRepository : IDailyGoalRepository
{
    private readonly PlannerDbContext context;

    public DailyGoalRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.DailyGoal> GetByIdAsync(Guid id)
    {
        return await context.DailyGoals.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.DailyGoal>> GetAllByGoalIdAsync(Guid goalId)
    {
        return await context.DailyGoals.Where(dailyGoal => dailyGoal.WeeklyGoalId == goalId).ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.DailyGoal dailyGoal)
    {
        await context.DailyGoals.AddAsync(dailyGoal);
    }

    public Task UpdateAsync(Domain.Entities.DailyGoal dailyGoal)
    {
        context.DailyGoals.Update(dailyGoal);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.DailyGoal dailyGoal = await context.DailyGoals.FindAsync(id);
        if (dailyGoal != null)
        {
            context.DailyGoals.Remove(dailyGoal);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}