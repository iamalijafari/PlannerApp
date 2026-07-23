using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.YearlyPlan;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.YearlyPlan;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.YearlyPlan;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YearlyPlanController : ControllerBase
{
    private readonly IYearlyPlanService yearlyPlanService;

    public YearlyPlanController(IYearlyPlanService yearlyPlanService)
    {
        this.yearlyPlanService = yearlyPlanService;
    }

    [HttpGet("by-goal/{goalId}")]
    public async Task<IActionResult> GetAllByGoalId(Guid goalId)
    {
        ServiceResult<IEnumerable<YearlyPlanDto>> result = await yearlyPlanService.GetAllByGoalIdAsync(goalId);
        return Ok(result.ToResponseModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<YearlyPlanDto> result = await yearlyPlanService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateYearlyPlanRequestModel dto)
    {
        ServiceResult<YearlyPlanDto> result = await yearlyPlanService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateYearlyPlanDto dto)
    {
        var updatedDto = new UpdateYearlyPlanDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await yearlyPlanService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await yearlyPlanService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await yearlyPlanService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}