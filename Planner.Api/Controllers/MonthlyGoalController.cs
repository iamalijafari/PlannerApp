using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.MonthlyGoal;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.MonthlyGoal;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.MonthlyGoal;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MonthlyGoalController : ControllerBase
{
    private readonly IMonthlyGoalService monthlyGoalService;

    public MonthlyGoalController(IMonthlyGoalService monthlyGoalService)
    {
        this.monthlyGoalService = monthlyGoalService;
    }

    [HttpGet("by-goal/{goalId}")]
    public async Task<IActionResult> GetAllByGoalId(Guid goalId)
    {
        ServiceResult<IEnumerable<MonthlyGoalDto>> result = await monthlyGoalService.GetAllByGoalIdAsync(goalId);
        return Ok(result.ToResponseModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<MonthlyGoalDto> result = await monthlyGoalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMonthlyGoalRequestModel dto)
    {
        ServiceResult<MonthlyGoalDto> result = await monthlyGoalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMonthlyGoalDto dto)
    {
        var updatedDto = new UpdateMonthlyGoalDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await monthlyGoalService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await monthlyGoalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await monthlyGoalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}