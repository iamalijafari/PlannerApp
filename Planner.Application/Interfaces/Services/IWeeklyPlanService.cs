using Planner.Application.DTOs.WeeklyPlan;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IWeeklyPlanService
{
    Task<ServiceResult<IEnumerable<WeeklyPlanDto>>> GetAllByMonthlyPlanIdAsync(Guid monthlyPlanId);
    Task<ServiceResult<WeeklyPlanDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<WeeklyPlanDto>> CreateAsync(CreateWeeklyPlanDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateWeeklyPlanDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}