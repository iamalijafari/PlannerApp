using Planner.Application.DTOs.DailyPlan;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IDailyPlanService
{
    Task<ServiceResult<IEnumerable<DailyPlanDto>>> GetAllByWeeklyPlanIdAsync(Guid weeklyPlanId);
    Task<ServiceResult<DailyPlanDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<DailyPlanDto>> CreateAsync(CreateDailyPlanDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateDailyPlanDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}