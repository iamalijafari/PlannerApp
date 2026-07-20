using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class WeeklyGoalRepository : IWeeklyGoalRepository
{
    private readonly PlannerDbContext context;

    public WeeklyGoalRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.WeeklyGoal> GetByIdAsync(Guid id)
    {
        return await context.WeeklyGoals.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.WeeklyGoal>> GetAllByGoalIdAsync(Guid goalId)
    {
        return await context.WeeklyGoals.Where(weeklyGoal => weeklyGoal.MonthlyGoalId == goalId).ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.WeeklyGoal weeklyGoal)
    {
        await context.WeeklyGoals.AddAsync(weeklyGoal);
    }

    public Task UpdateAsync(Domain.Entities.WeeklyGoal weeklyGoal)
    {
        context.WeeklyGoals.Update(weeklyGoal);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.WeeklyGoal weeklyGoal = await context.WeeklyGoals.FindAsync(id);
        if (weeklyGoal != null)
        {
            context.WeeklyGoals.Remove(weeklyGoal);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}