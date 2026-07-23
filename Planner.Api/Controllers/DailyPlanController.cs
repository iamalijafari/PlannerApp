using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.DailyPlan;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.DailyPlan;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.DailyPlan;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DailyPlanController : ControllerBase
{
    private readonly IDailyPlanService dailyPlanService;

    public DailyPlanController(IDailyPlanService dailyPlanService)
    {
        this.dailyPlanService = dailyPlanService;
    }

    [HttpGet("by-weekly-plan/{weeklyPlanId}")]
    public async Task<IActionResult> GetAllByWeeklyPlanId(Guid weeklyPlanId)
    {
        ServiceResult<IEnumerable<DailyPlanDto>> result = await dailyPlanService.GetAllByWeeklyPlanIdAsync(weeklyPlanId);
        return Ok(result.ToResponseModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<DailyPlanDto> result = await dailyPlanService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDailyPlanRequestModel dto)
    {
        ServiceResult<DailyPlanDto> result = await dailyPlanService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDailyPlanDto dto)
    {
        var updatedDto = new UpdateDailyPlanDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await dailyPlanService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await dailyPlanService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await dailyPlanService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}