using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IMonthlyGoalRepository
{
    Task<MonthlyGoal> GetByIdAsync(Guid id);
    Task<IEnumerable<MonthlyGoal>> GetAllByGoalIdAsync(Guid goalId);
    Task AddAsync(MonthlyGoal goal);
    Task UpdateAsync(MonthlyGoal goal);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}