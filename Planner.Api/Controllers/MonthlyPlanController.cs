using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.MonthlyPlan;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.MonthlyPlan;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.MonthlyPlan;

namespace Planner.Api.Controllers;

/// <summary>
/// Manages monthly plans that belong to a yearly plan.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MonthlyPlanController : ControllerBase
{
    private readonly IMonthlyPlanService monthlyPlanService;

    public MonthlyPlanController(IMonthlyPlanService monthlyPlanService)
    {
        this.monthlyPlanService = monthlyPlanService;
    }

    /// <summary>Returns a yearly plan's monthly plans ordered by due date.</summary>
    [HttpGet("by-yearly-plan/{yearlyPlanId}")]
    public async Task<IActionResult> GetAllByYearlyPlanId(Guid yearlyPlanId)
    {
        ServiceResult<IEnumerable<MonthlyPlanDto>> result = await monthlyPlanService.GetAllByYearlyPlanIdAsync(yearlyPlanId);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Returns one monthly plan by its identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<MonthlyPlanDto> result = await monthlyPlanService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Creates a monthly plan.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMonthlyPlanRequestModel dto)
    {
        ServiceResult<MonthlyPlanDto> result = await monthlyPlanService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    /// <summary>Updates a monthly plan.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMonthlyPlanDto dto)
    {
        var updatedDto = new UpdateMonthlyPlanDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await monthlyPlanService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Deletes a monthly plan.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await monthlyPlanService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Marks a monthly plan as completed.</summary>
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await monthlyPlanService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}
