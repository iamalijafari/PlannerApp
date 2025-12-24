using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IYearlyGoalRepository
{
    Task<YearlyGoal> GetByIdAsync(Guid id);
    Task<IEnumerable<YearlyGoal>> GetAllAsync();
    Task AddAsync(YearlyGoal goal);
    Task UpdateAsync(YearlyGoal goal);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}