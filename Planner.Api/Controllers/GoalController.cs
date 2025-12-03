using Microsoft.AspNetCore.Mvc;
using Planner.Application.Interfaces.Services;
using Planner.Application.DTOs.Goal;
using Planner.Application.DTOs.Utility;
using System.Collections.Generic;
using Planner.Api.Mappers.Utilities;

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

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        ServiceResult<IEnumerable<GoalDto>> result = await goalService.GetAllAsync();
        return result.ToResult();
    }

    [HttpGet("{id}")]
    public async Task<IResult> Get(Guid id)
    {
        ServiceResult<GoalDto> result = await goalService.GetByIdAsync(id);
        return result.ToResult();
    }

    [HttpPost]
    public async Task<IResult> Create(CreateGoalDto dto)
    {
        ServiceResult<GoalDto> result = await goalService.CreateAsync(dto);
        return result.ToResult();
    }

    [HttpPut("{id}")]
    public async Task<IResult> Update(UpdateGoalDto dto)
    {
        ServiceResult<bool> result = await goalService.UpdateAsync(dto);
        return result.ToResult();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(Guid id)
    {
        ServiceResult<bool> result = await goalService.DeleteAsync(id);
        return result.ToResult();
    }
}