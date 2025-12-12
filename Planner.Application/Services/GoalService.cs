using Planner.Application.DTOs.Goal;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.Goal;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;

public class GoalService : IGoalService
{
    private readonly IGoalRepository goalRepository;

    public GoalService(IGoalRepository goalRepository)
    {
        this.goalRepository = goalRepository;
    }
        
    public async Task<ServiceResult<IEnumerable<GoalDto>>> GetAllAsync()
    {
        ServiceResult<IEnumerable<GoalDto>> result = new();
        IEnumerable<Goal> goals = await goalRepository.GetAllAsync();
        result.SetResult(goals.ToDto());
        return result;
    }

    public async Task<ServiceResult<GoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<GoalDto> result = new();
        Goal goal = await goalRepository.GetByIdAsync(id);
        result.SetResult(goal.ToDto());
        return result;
    }

    public async Task<ServiceResult<GoalDto>> CreateAsync(CreateGoalDto dto)
    {
        ServiceResult<GoalDto> result = new();
        Goal goal = new Goal(dto.Title, dto.Description, dto.DueDate);
        await goalRepository.AddAsync(goal);
        await goalRepository.SaveChangesAsync();
        result.SetResult(goal.ToDto());
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateGoalDto dto)
    {
        ServiceResult<bool> result = new();
        Goal goal = await goalRepository.GetByIdAsync(dto.Id);
        goal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        await goalRepository.UpdateAsync(goal);
        await goalRepository.SaveChangesAsync();
        result.SetResult(true);
        return result;
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
    {
        ServiceResult<bool> result = new();
        await goalRepository.DeleteAsync(id);
        await goalRepository.SaveChangesAsync();
        result.SetResult(true);
        return result;
    }
}