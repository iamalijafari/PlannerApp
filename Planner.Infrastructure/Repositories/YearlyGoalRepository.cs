using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class YearlyGoalRepository : IYearlyGoalRepository
{
    private readonly PlannerDbContext context;

    public YearlyGoalRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.YearlyGoal> GetByIdAsync(Guid id)
    {
        return await context.YearlyGoals.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.YearlyGoal>> GetAllByGoalIdAsync(Guid goalId)
    {
        return await context.YearlyGoals.Where(yearlyGoal => yearlyGoal.GoalId == goalId).ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.YearlyGoal yearlyGoal)
    {
        await context.YearlyGoals.AddAsync(yearlyGoal);
    }

    public Task UpdateAsync(Domain.Entities.YearlyGoal yearlyGoal)
    {
        context.YearlyGoals.Update(yearlyGoal);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.YearlyGoal yearlyGoal = await context.YearlyGoals.FindAsync(id);
        if (yearlyGoal != null)
        {
            context.YearlyGoals.Remove(yearlyGoal);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}