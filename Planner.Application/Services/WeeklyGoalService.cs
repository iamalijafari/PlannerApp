using Planner.Application.DTOs.WeeklyGoal;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.WeeklyGoal;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;

namespace Planner.Application.Services;

public class WeeklyGoalService : IWeeklyGoalService
{
    private readonly IWeeklyGoalRepository weeklyGoalRepository;

    public WeeklyGoalService(IWeeklyGoalRepository weeklyGoalRepository)
    {
        this.weeklyGoalRepository = weeklyGoalRepository;
    }

    public async Task<ServiceResult<IEnumerable<WeeklyGoalDto>>> GetAllByGoalIdAsync(Guid goalId)
    {
        ServiceResult<IEnumerable<WeeklyGoalDto>> result = new();
        try
        {
            if (goalId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<WeeklyGoal> weeklyGoals = await weeklyGoalRepository.GetAllByGoalIdAsync(goalId);
            result.SetResult(weeklyGoals.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<WeeklyGoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<WeeklyGoalDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            WeeklyGoal weeklyGoal = await weeklyGoalRepository.GetByIdAsync(id);
            if (weeklyGoal == null)
            {
                result.SetError(MessageKey.WeeklyGoal_NotFound);
                return result;
            }

            result.SetResult(weeklyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<WeeklyGoalDto>> CreateAsync(CreateWeeklyGoalDto dto)
    {
        ServiceResult<WeeklyGoalDto> result = new();
        try
        {
            if (dto == null || dto.GoalId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            WeeklyGoal weeklyGoal = new WeeklyGoal(dto.GoalId, dto.Title, dto.Description, dto.DueDate);
            await weeklyGoalRepository.AddAsync(weeklyGoal);
            await weeklyGoalRepository.SaveChangesAsync();
            result.SetResult(weeklyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateWeeklyGoalDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            WeeklyGoal weeklyGoal = await weeklyGoalRepository.GetByIdAsync(dto.Id);
            if (weeklyGoal == null)
            {
                result.SetError(MessageKey.WeeklyGoal_NotFound);
                return result;
            }

            weeklyGoal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await weeklyGoalRepository.UpdateAsync(weeklyGoal);
            await weeklyGoalRepository.SaveChangesAsync();
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

            WeeklyGoal weeklyGoal = await weeklyGoalRepository.GetByIdAsync(id);
            if (weeklyGoal == null)
            {
                result.SetError(MessageKey.WeeklyGoal_NotFound);
                return result;
            }

            await weeklyGoalRepository.DeleteAsync(id);
            await weeklyGoalRepository.SaveChangesAsync();
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

            WeeklyGoal weeklyGoal = await weeklyGoalRepository.GetByIdAsync(id);
            if (weeklyGoal == null)
            {
                result.SetError(MessageKey.WeeklyGoal_NotFound);
                return result;
            }

            weeklyGoal.Update(weeklyGoal.Title, weeklyGoal.Description, weeklyGoal.DueDate, true);
            await weeklyGoalRepository.UpdateAsync(weeklyGoal);
            await weeklyGoalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }
}