using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IWeeklyGoalRepository
{
    Task<WeeklyGoal> GetByIdAsync(Guid id);
    Task<IEnumerable<WeeklyGoal>> GetAllByGoalIdAsync(Guid goalId);
    Task AddAsync(WeeklyGoal goal);
    Task UpdateAsync(WeeklyGoal goal);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}