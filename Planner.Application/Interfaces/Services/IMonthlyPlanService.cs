using Planner.Application.DTOs.MonthlyPlan;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IMonthlyPlanService
{
    Task<ServiceResult<IEnumerable<MonthlyPlanDto>>> GetAllByYearlyPlanIdAsync(Guid yearlyPlanId);
    Task<ServiceResult<MonthlyPlanDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<MonthlyPlanDto>> CreateAsync(CreateMonthlyPlanDto dto);
    Task<ServiceResult<bool>> UpdateAsync(UpdateMonthlyPlanDto dto);
    Task<ServiceResult<bool>> DeleteAsync(Guid id);
    Task<ServiceResult<bool>> CompleteAsync(Guid id);
}