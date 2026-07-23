using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class YearlyPlanRepository : IYearlyPlanRepository
{
    private readonly PlannerDbContext context;

    public YearlyPlanRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.YearlyPlan> GetByIdAsync(Guid id)
    {
        return await context.YearlyPlans.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.YearlyPlan>> GetAllByGoalIdAsync(Guid goalId)
    {
        return await context.YearlyPlans
            .Where(yearlyPlan => yearlyPlan.GoalId == goalId)
            .OrderBy(yearlyPlan => yearlyPlan.DueDate)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.YearlyPlan yearlyPlan)
    {
        await context.YearlyPlans.AddAsync(yearlyPlan);
    }

    public Task UpdateAsync(Domain.Entities.YearlyPlan yearlyPlan)
    {
        context.YearlyPlans.Update(yearlyPlan);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.YearlyPlan yearlyPlan = await context.YearlyPlans.FindAsync(id);
        if (yearlyPlan != null)
        {
            context.YearlyPlans.Remove(yearlyPlan);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
