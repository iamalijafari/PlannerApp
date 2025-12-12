using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.Goal;
using Planner.Application.DTOs.Utility;
using System.Collections.Generic;
using Planner.Api.Mappers.Goal;
using Planner.Api.Mappers.General;
using Planner.Api.DTOs.Requests.Goal;

namespace Planner.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalController : ControllerBase
{
    private readonly IGoalService goalService;

    public GoalController(IGoalService goalService)
    {
        this.goalService = goalService;
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        ServiceResult<IEnumerable<GoalDto>> result = await goalService.GetAllAsync();
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Get")]
    public async Task<IActionResult> Get([FromBody] Guid id)
    {
        ServiceResult<GoalDto> result = await goalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateGoalRequestModel dto)
    {
        ServiceResult<GoalDto> result = await goalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateGoalDto dto)
    {
        ServiceResult<bool> result = await goalService.UpdateAsync(dto);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        ServiceResult<bool> result = await goalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> Complete([FromBody] Guid id)
    {
        ServiceResult<bool> result = await goalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}