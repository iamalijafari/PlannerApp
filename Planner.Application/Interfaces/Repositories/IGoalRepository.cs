using Planner.Domain.Entities;

namespace Planner.Application.Interfaces.Repositories;

public interface IGoalRepository
{
    Task<Goal> GetByIdAsync(Guid id);
    Task<IEnumerable<Goal>> GetAllAsync();
    Task AddAsync(Goal goal);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}