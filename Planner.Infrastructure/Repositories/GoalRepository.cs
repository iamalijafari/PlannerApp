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
        return await context.Goals
            .OrderBy(goal => goal.DueDate)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.Goal goal)
    {
        await context.Goals.AddAsync(goal);
    }

    public Task UpdateAsync(Domain.Entities.Goal goal)
    {
        context.Goals.Update(goal);
        return Task.CompletedTask;
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

    public async Task<Domain.Entities.Goal> GetTreeByIdAsync(Guid id)
    {
        return await context.Goals
            .Include(g => g.YearlyPlans)
                .ThenInclude(y => y.MonthlyPlans)
                    .ThenInclude(m => m.WeeklyPlans)
                        .ThenInclude(w => w.DailyPlans)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Domain.Entities.Goal>> GetAllWithPlansAsync()
    {
        return await context.Goals
            .AsNoTracking()
            .AsSplitQuery()
            .Include(g => g.YearlyPlans)
                .ThenInclude(y => y.MonthlyPlans)
                    .ThenInclude(m => m.WeeklyPlans)
                        .ThenInclude(w => w.DailyPlans)
            .OrderBy(goal => goal.DueDate)
            .ToListAsync();
    }
}
