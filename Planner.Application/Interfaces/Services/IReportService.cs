using Planner.Application.DTOs.Report;
using Planner.Application.DTOs.Utility;

namespace Planner.Application.Interfaces.Services;

public interface IReportService
{
    Task<ServiceResult<GoalsProgressReportDto>> GetGoalsProgressAsync();
}
