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
public class SubGoalController : ControllerBase
{
    private readonly ISubGoalService yearlyGoalService;

    public SubGoalController(ISubGoalService yearlyGoalService)
    {
        this.yearlyGoalService = yearlyGoalService;
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        ServiceResult<IEnumerable<SubGoalDto>> result = await yearlyGoalService.GetAllAsync();
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Get")]
    public async Task<IActionResult> Get([FromBody] Guid id)
    {
        ServiceResult<SubGoalDto> result = await yearlyGoalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateSubGoalRequestModel dto)
    {
        ServiceResult<SubGoalDto> result = await yearlyGoalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateSubGoalDto dto)
    {
        ServiceResult<bool> result = await yearlyGoalService.UpdateAsync(dto);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        ServiceResult<bool> result = await yearlyGoalService.DeleteAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Complete")]
    public async Task<IActionResult> Complete([FromBody] Guid id)
    {
        ServiceResult<bool> result = await yearlyGoalService.CompleteAsync(id);
        return Ok(result.ToResponseModel());
    }
}