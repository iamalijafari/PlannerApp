using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.WeeklyPlan;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.WeeklyPlan;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.WeeklyPlan;

namespace Planner.Api.Controllers;

/// <summary>
/// Manages weekly plans that belong to a monthly plan.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WeeklyPlanController : ControllerBase
{
    private readonly IWeeklyPlanService weeklyPlanService;

    public WeeklyPlanController(IWeeklyPlanService weeklyPlanService)
    {
        this.weeklyPlanService = weeklyPlanService;
    }

    /// <summary>Returns a monthly plan's weekly plans ordered by due date.</summary>
    [HttpGet("by-monthly-plan/{monthlyPlanId}")]
    public async Task<IActionResult> GetAllByMonthlyPlanId(Guid monthlyPlanId)
    {
        ServiceResult<IEnumerable<WeeklyPlanDto>> result = await weeklyPlanService.GetAllByMonthlyPlanIdAsync(monthlyPlanId);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Returns one weekly plan by its identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<WeeklyPlanDto> result = await weeklyPlanService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Creates a weekly plan.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWeeklyPlanRequestModel dto)
    {
        ServiceResult<WeeklyPlanDto> result = await weeklyPlanService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    /// <summary>Updates a weekly plan.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWeeklyPlanDto dto)
    {
        var updatedDto = new UpdateWeeklyPlanDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await weeklyPlanService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Deletes a weekly plan.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await weeklyPlanService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Marks a weekly plan as completed.</summary>
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await weeklyPlanService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}
