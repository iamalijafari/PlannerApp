using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IDailyPlanRepository
{
    Task<DailyPlan> GetByIdAsync(Guid id);
    Task<IEnumerable<DailyPlan>> GetAllByWeeklyPlanIdAsync(Guid weeklyPlanId);
    Task AddAsync(DailyPlan plan);
    Task UpdateAsync(DailyPlan plan);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}