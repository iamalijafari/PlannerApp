using Planner.Application.DTOs.Goal;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IGoalService
{
    Task<ServiceResult<IEnumerable<GoalDto>>> GetAllAsync();
    Task<ServiceResult<GoalDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<GoalDto>> CreateAsync(CreateGoalDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateGoalDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
    Task<ServiceResult<GoalTreeDto>> GetTreeAsync(Guid id);
}