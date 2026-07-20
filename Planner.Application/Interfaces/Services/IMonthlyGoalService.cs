using Planner.Application.DTOs.MonthlyGoal;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IMonthlyGoalService
{
    Task<ServiceResult<IEnumerable<MonthlyGoalDto>>> GetAllByGoalIdAsync(Guid goalId);
    Task<ServiceResult<MonthlyGoalDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<MonthlyGoalDto>> CreateAsync(CreateMonthlyGoalDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateMonthlyGoalDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}