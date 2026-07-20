using Planner.Application.DTOs.DailyGoal;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IDailyGoalService
{
    Task<ServiceResult<IEnumerable<DailyGoalDto>>> GetAllByGoalIdAsync(Guid goalId);
    Task<ServiceResult<DailyGoalDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<DailyGoalDto>> CreateAsync(CreateDailyGoalDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateDailyGoalDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}