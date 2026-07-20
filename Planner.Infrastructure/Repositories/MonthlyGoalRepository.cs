using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class MonthlyGoalRepository : IMonthlyGoalRepository
{
    private readonly PlannerDbContext context;

    public MonthlyGoalRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.MonthlyGoal> GetByIdAsync(Guid id)
    {
        return await context.MonthlyGoals.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.MonthlyGoal>> GetAllByGoalIdAsync(Guid goalId)
    {
        return await context.MonthlyGoals.Where(monthlyGoal => monthlyGoal.YearlyGoalId == goalId).ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.MonthlyGoal monthlyGoal)
    {
        await context.MonthlyGoals.AddAsync(monthlyGoal);
    }

    public Task UpdateAsync(Domain.Entities.MonthlyGoal monthlyGoal)
    {
        context.MonthlyGoals.Update(monthlyGoal);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.MonthlyGoal monthlyGoal = await context.MonthlyGoals.FindAsync(id);
        if (monthlyGoal != null)
        {
            context.MonthlyGoals.Remove(monthlyGoal);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}