using Planner.Application.DTOs.WeeklyGoal;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IWeeklyGoalService
{
    Task<ServiceResult<IEnumerable<WeeklyGoalDto>>> GetAllByGoalIdAsync(Guid goalId);
    Task<ServiceResult<WeeklyGoalDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<WeeklyGoalDto>> CreateAsync(CreateWeeklyGoalDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateWeeklyGoalDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}