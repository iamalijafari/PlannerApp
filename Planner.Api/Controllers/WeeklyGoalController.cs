using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.WeeklyGoal;
using Planner.Application.DTOs.Utility;
using System.Collections.Generic;
using Planner.Api.Mappers.WeeklyGoal;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.WeeklyGoal;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeeklyGoalController : ControllerBase
{
    private readonly IWeeklyGoalService weeklyGoalService;

    public WeeklyGoalController(IWeeklyGoalService weeklyGoalService)
    {
        this.weeklyGoalService = weeklyGoalService;
    }

    [HttpGet("by-goal/{goalId}")]
    public async Task<IActionResult> GetAllByGoalId(Guid goalId)
    {
        ServiceResult<IEnumerable<WeeklyGoalDto>> result = await weeklyGoalService.GetAllByGoalIdAsync(goalId);
        return Ok(result.ToResponseModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<WeeklyGoalDto> result = await weeklyGoalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWeeklyGoalRequestModel dto)
    {
        ServiceResult<WeeklyGoalDto> result = await weeklyGoalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWeeklyGoalDto dto)
    {
        var updatedDto = new UpdateWeeklyGoalDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await weeklyGoalService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await weeklyGoalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await weeklyGoalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}