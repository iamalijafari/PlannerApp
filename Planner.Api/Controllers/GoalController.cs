using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.Goal;
using Planner.Application.DTOs.Utility;
using Planner.Api.Mappers.Goal;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.Goal;

namespace Planner.Api.Controllers;

/// <summary>
/// Manages top-level goals and their complete planning hierarchy.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GoalController : ControllerBase
{
    private readonly IGoalService goalService;

    public GoalController(IGoalService goalService)
    {
        this.goalService = goalService;
    }

    /// <summary>Returns every goal ordered by due date.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        ServiceResult<IEnumerable<GoalDto>> result = await goalService.GetAllAsync();
        return Ok(result.ToResponseModel());
    }

    /// <summary>Returns one goal by its identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        ServiceResult<GoalDto> result = await goalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Creates a top-level goal.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGoalRequestModel dto)
    {
        ServiceResult<GoalDto> result = await goalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    /// <summary>Updates a top-level goal.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGoalDto dto)
    {
        var updatedDto = new UpdateGoalDto(id, dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        ServiceResult<bool> result = await goalService.UpdateAsync(updatedDto);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Deletes a top-level goal.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await goalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Marks a top-level goal as completed.</summary>
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        ServiceResult<bool> result = await goalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    /// <summary>Returns the complete goal hierarchy ordered by due date at every level.</summary>
    [HttpGet("{id}/tree")]
    public async Task<IActionResult> GetTree(Guid id)
    {
        ServiceResult<GoalTreeDto> result = await goalService.GetTreeAsync(id);
        return Ok(result);
    }
}
