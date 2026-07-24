using Microsoft.AspNetCore.Mvc;
using Planner.Api.Mappers.Report;
using Planner.Application.DTOs.Report;
using Planner.Application.DTOs.Utility;
using Planner.Application.Interfaces.Services;

namespace Planner.Api.Controllers;

/// <summary>
/// Provides aggregated reporting data for goals and their deepest plans.
/// </summary>
[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private readonly IReportService reportService;

    public ReportController(IReportService reportService)
    {
        this.reportService = reportService;
    }

    /// <summary>
    /// Returns goal statuses and progress calculated from completed leaf plans.
    /// </summary>
    [HttpGet("goals-progress")]
    public async Task<IActionResult> GetGoalsProgress()
    {
        ServiceResult<GoalsProgressReportDto> result =
            await reportService.GetGoalsProgressAsync();

        return Ok(result.ToResponseModel());
    }
}
