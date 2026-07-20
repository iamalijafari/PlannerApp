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
        try
        {
            IEnumerable<Goal> goals = await goalRepository.GetAllAsync();
            result.SetResult(goals.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<GoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<GoalDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            Goal goal = await goalRepository.GetByIdAsync(id);
            if (goal == null)
            {
                result.SetError(MessageKey.Goal_NotFound);
                return result;
            }

            result.SetResult(goal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<GoalDto>> CreateAsync(CreateGoalDto dto)
    {
        ServiceResult<GoalDto> result = new();
        try
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            Goal goal = new Goal(dto.Title, dto.Description, dto.DueDate);
            await goalRepository.AddAsync(goal);
            await goalRepository.SaveChangesAsync();
            result.SetResult(goal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateGoalDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            Goal goal = await goalRepository.GetByIdAsync(dto.Id);
            if (goal == null)
            {
                result.SetError(MessageKey.Goal_NotFound);
                return result;
            }

            goal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await goalRepository.UpdateAsync(goal);
            await goalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            Goal goal = await goalRepository.GetByIdAsync(id);
            if (goal == null)
            {
                result.SetError(MessageKey.Goal_NotFound);
                return result;
            }

            await goalRepository.DeleteAsync(id);
            await goalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> CompleteAsync(Guid id)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            Goal goal = await goalRepository.GetByIdAsync(id);
            if (goal == null)
            {
                result.SetError(MessageKey.Goal_NotFound);
                return result;
            }

            goal.Update(goal.Title, goal.Description, goal.DueDate, true);
            await goalRepository.UpdateAsync(goal);
            await goalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<GoalTreeDto>> GetTreeAsync(Guid id)
    {
        ServiceResult<GoalTreeDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            Goal goal = await goalRepository.GetTreeByIdAsync(id);
            if (goal == null)
            {
                result.SetError(MessageKey.Goal_NotFound);
                return result;
            }

            result.SetResult(goal.ToTreeDto());
        }
        catch (Exception)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }
}