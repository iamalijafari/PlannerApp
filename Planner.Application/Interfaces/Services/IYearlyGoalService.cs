using Planner.Application.DTOs.YearlyGoal;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IYearlyGoalService
{
    Task<ServiceResult<IEnumerable<YearlyGoalDto>>> GetAllByGoalIdAsync(Guid goalId);
    Task<ServiceResult<YearlyGoalDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<YearlyGoalDto>> CreateAsync(CreateYearlyGoalDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateYearlyGoalDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}