using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IDailyGoalRepository
{
    Task<DailyGoal> GetByIdAsync(Guid id);
    Task<IEnumerable<DailyGoal>> GetAllByGoalIdAsync(Guid goalId);
    Task AddAsync(DailyGoal goal);
    Task UpdateAsync(DailyGoal goal);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}