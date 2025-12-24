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

    [HttpPost("GetAllByGoalId")]
    public async Task<IActionResult> GetAllByGoalId([FromBody] Guid goalId)
    {
        ServiceResult<IEnumerable<YearlyGoalDto>> result = await yearlyGoalService.GetAllByGoalIdAsync(goalId);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Get")]
    public async Task<IActionResult> Get([FromBody] Guid id)
    {
        ServiceResult<YearlyGoalDto> result = await yearlyGoalService.GetByIdAsync(id);
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateYearlyGoalRequestModel dto)
    {
        ServiceResult<YearlyGoalDto> result = await yearlyGoalService.CreateAsync(dto.ToDto());
        return Ok(result.ToResponseModel());
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateYearlyGoalDto dto)
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