using Planner.Application.DTOs.YearlyPlan;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IYearlyPlanService
{
    Task<ServiceResult<IEnumerable<YearlyPlanDto>>> GetAllByGoalIdAsync(Guid goalId);
    Task<ServiceResult<YearlyPlanDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<YearlyPlanDto>> CreateAsync(CreateYearlyPlanDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateYearlyPlanDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}