using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IWeeklyPlanRepository
{
    Task<WeeklyPlan> GetByIdAsync(Guid id);
    Task<IEnumerable<WeeklyPlan>> GetAllByMonthlyPlanIdAsync(Guid monthlyPlanId);
    Task AddAsync(WeeklyPlan plan);
    Task UpdateAsync(WeeklyPlan plan);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}