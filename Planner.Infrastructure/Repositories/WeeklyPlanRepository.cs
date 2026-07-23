using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class WeeklyPlanRepository : IWeeklyPlanRepository
{
    private readonly PlannerDbContext context;

    public WeeklyPlanRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.WeeklyPlan> GetByIdAsync(Guid id)
    {
        return await context.WeeklyPlans.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.WeeklyPlan>> GetAllByMonthlyPlanIdAsync(Guid monthlyPlanId)
    {
        return await context.WeeklyPlans
            .Where(weeklyPlan => weeklyPlan.MonthlyPlanId == monthlyPlanId)
            .OrderBy(weeklyPlan => weeklyPlan.DueDate)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.WeeklyPlan weeklyPlan)
    {
        await context.WeeklyPlans.AddAsync(weeklyPlan);
    }

    public Task UpdateAsync(Domain.Entities.WeeklyPlan weeklyPlan)
    {
        context.WeeklyPlans.Update(weeklyPlan);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.WeeklyPlan weeklyPlan = await context.WeeklyPlans.FindAsync(id);
        if (weeklyPlan != null)
        {
            context.WeeklyPlans.Remove(weeklyPlan);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
