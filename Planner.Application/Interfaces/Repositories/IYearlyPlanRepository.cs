using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IYearlyPlanRepository
{
    Task<YearlyPlan> GetByIdAsync(Guid id);
    Task<IEnumerable<YearlyPlan>> GetAllByGoalIdAsync(Guid goalId);
    Task AddAsync(YearlyPlan plan);
    Task UpdateAsync(YearlyPlan plan);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}