using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class DailyPlanRepository : IDailyPlanRepository
{
    private readonly PlannerDbContext context;

    public DailyPlanRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.DailyPlan> GetByIdAsync(Guid id)
    {
        return await context.DailyPlans.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.DailyPlan>> GetAllByWeeklyPlanIdAsync(Guid weeklyPlanId)
    {
        return await context.DailyPlans.Where(dailyPlan => dailyPlan.WeeklyPlanId == weeklyPlanId).ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.DailyPlan dailyPlan)
    {
        await context.DailyPlans.AddAsync(dailyPlan);
    }

    public Task UpdateAsync(Domain.Entities.DailyPlan dailyPlan)
    {
        context.DailyPlans.Update(dailyPlan);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.DailyPlan dailyPlan = await context.DailyPlans.FindAsync(id);
        if (dailyPlan != null)
        {
            context.DailyPlans.Remove(dailyPlan);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}