using Planner.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Planner.Infrastructure.Persistence;

namespace Planner.Infrastructure.Repositories;

public class MonthlyPlanRepository : IMonthlyPlanRepository
{
    private readonly PlannerDbContext context;

    public MonthlyPlanRepository(PlannerDbContext context)
    {
        this.context = context;
    }

    public async Task<Domain.Entities.MonthlyPlan> GetByIdAsync(Guid id)
    {
        return await context.MonthlyPlans.FindAsync(id);
    }

    public async Task<IEnumerable<Domain.Entities.MonthlyPlan>> GetAllByYearlyPlanIdAsync(Guid yearlyPlanId)
    {
        return await context.MonthlyPlans
            .Where(monthlyPlan => monthlyPlan.YearlyPlanId == yearlyPlanId)
            .OrderBy(monthlyPlan => monthlyPlan.DueDate)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.MonthlyPlan monthlyPlan)
    {
        await context.MonthlyPlans.AddAsync(monthlyPlan);
    }

    public Task UpdateAsync(Domain.Entities.MonthlyPlan monthlyPlan)
    {
        context.MonthlyPlans.Update(monthlyPlan);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        Domain.Entities.MonthlyPlan monthlyPlan = await context.MonthlyPlans.FindAsync(id);
        if (monthlyPlan != null)
        {
            context.MonthlyPlans.Remove(monthlyPlan);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
