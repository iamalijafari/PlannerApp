using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IMonthlyPlanRepository
{
    Task<MonthlyPlan> GetByIdAsync(Guid id);
    Task<IEnumerable<MonthlyPlan>> GetAllByYearlyPlanIdAsync(Guid yearlyPlanId);
    Task AddAsync(MonthlyPlan plan);
    Task UpdateAsync(MonthlyPlan plan);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}