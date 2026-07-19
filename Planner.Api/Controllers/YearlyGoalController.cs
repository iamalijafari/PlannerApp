using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.YearlyGoal;
using Planner.Application.DTOs.Utility;
using System.Collections.Generic;
using Planner.Api.Mappers.YearlyGoal;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.YearlyGoal;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YearlyGoalController : ControllerBase
{
    private readonly IYearlyGoalService yearlyGoalService;

    public YearlyGoalController(IYearlyGoalService yearlyGoalService)
    {
        this.yearlyGoalService = yearlyGoalService;
    }

    [HttpGet("by-goal/{goalId}")]
    public async Task<IActionResult> GetAllByGoalId(Guid goalId)
    {
        ServiceResult<IEnumerable<YearlyGoalDto>> result = await yearlyGoalService.GetAllByGoalIdAsync(goalId);
        return Ok(result.ToResponseModel());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<YearlyGoalDto> result = await yearlyGoalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateYearlyGoalRequestModel dto)
    {
        ServiceResult<YearlyGoalDto> result = await yearlyGoalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateYearlyGoalDto dto)
    {
        // Reconstruct DTO with the ID from the route
        var updatedDto = new UpdateYearlyGoalDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await yearlyGoalService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await yearlyGoalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await yearlyGoalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}