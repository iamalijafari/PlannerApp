using Planner.Application.DTOs.DailyGoal;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.DailyGoal;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;

namespace Planner.Application.Services;

public class DailyGoalService : IDailyGoalService
{
    private readonly IDailyGoalRepository dailyGoalRepository;

    public DailyGoalService(IDailyGoalRepository dailyGoalRepository)
    {
        this.dailyGoalRepository = dailyGoalRepository;
    }

    public async Task<ServiceResult<IEnumerable<DailyGoalDto>>> GetAllByGoalIdAsync(Guid goalId)
    {
        ServiceResult<IEnumerable<DailyGoalDto>> result = new();
        try
        {
            if (goalId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<DailyGoal> dailyGoals = await dailyGoalRepository.GetAllByGoalIdAsync(goalId);
            result.SetResult(dailyGoals.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<DailyGoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<DailyGoalDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            DailyGoal dailyGoal = await dailyGoalRepository.GetByIdAsync(id);
            if (dailyGoal == null)
            {
                result.SetError(MessageKey.DailyGoal_NotFound);
                return result;
            }

            result.SetResult(dailyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<DailyGoalDto>> CreateAsync(CreateDailyGoalDto dto)
    {
        ServiceResult<DailyGoalDto> result = new();
        try
        {
            if (dto == null || dto.GoalId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            DailyGoal dailyGoal = new DailyGoal(dto.GoalId, dto.Title, dto.Description, dto.DueDate);
            await dailyGoalRepository.AddAsync(dailyGoal);
            await dailyGoalRepository.SaveChangesAsync();
            result.SetResult(dailyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateDailyGoalDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            DailyGoal dailyGoal = await dailyGoalRepository.GetByIdAsync(dto.Id);
            if (dailyGoal == null)
            {
                result.SetError(MessageKey.DailyGoal_NotFound);
                return result;
            }

            dailyGoal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await dailyGoalRepository.UpdateAsync(dailyGoal);
            await dailyGoalRepository.SaveChangesAsync();
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

            DailyGoal dailyGoal = await dailyGoalRepository.GetByIdAsync(id);
            if (dailyGoal == null)
            {
                result.SetError(MessageKey.DailyGoal_NotFound);
                return result;
            }

            await dailyGoalRepository.DeleteAsync(id);
            await dailyGoalRepository.SaveChangesAsync();
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

            DailyGoal dailyGoal = await dailyGoalRepository.GetByIdAsync(id);
            if (dailyGoal == null)
            {
                result.SetError(MessageKey.DailyGoal_NotFound);
                return result;
            }

            dailyGoal.Update(dailyGoal.Title, dailyGoal.Description, dailyGoal.DueDate, true);
            await dailyGoalRepository.UpdateAsync(dailyGoal);
            await dailyGoalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }
}