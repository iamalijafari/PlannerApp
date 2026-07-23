using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.DailyGoal;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.DailyGoal;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.DailyGoal;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DailyGoalController : ControllerBase
{
    private readonly IDailyGoalService dailyGoalService;

    public DailyGoalController(IDailyGoalService dailyGoalService)
    {
        this.dailyGoalService = dailyGoalService;
    }

    [HttpGet("by-goal/{goalId}")]
    public async Task<IActionResult> GetAllByGoalId(Guid goalId)
    {
        ServiceResult<IEnumerable<DailyGoalDto>> result = await dailyGoalService.GetAllByGoalIdAsync(goalId);
        return Ok(result.ToResponseModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<DailyGoalDto> result = await dailyGoalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDailyGoalRequestModel dto)
    {
        ServiceResult<DailyGoalDto> result = await dailyGoalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDailyGoalDto dto)
    {
        var updatedDto = new UpdateDailyGoalDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await dailyGoalService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await dailyGoalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await dailyGoalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}