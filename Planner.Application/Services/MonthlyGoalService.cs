using Planner.Application.DTOs.MonthlyGoal;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.MonthlyGoal;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;

namespace Planner.Application.Services;

public class MonthlyGoalService : IMonthlyGoalService
{
    private readonly IMonthlyGoalRepository monthlyGoalRepository;

    public MonthlyGoalService(IMonthlyGoalRepository monthlyGoalRepository)
    {
        this.monthlyGoalRepository = monthlyGoalRepository;
    }

    public async Task<ServiceResult<IEnumerable<MonthlyGoalDto>>> GetAllByGoalIdAsync(Guid goalId)
    {
        ServiceResult<IEnumerable<MonthlyGoalDto>> result = new();
        try
        {
            if (goalId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<MonthlyGoal> monthlyGoals = await monthlyGoalRepository.GetAllByGoalIdAsync(goalId);
            result.SetResult(monthlyGoals.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<MonthlyGoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<MonthlyGoalDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            MonthlyGoal monthlyGoal = await monthlyGoalRepository.GetByIdAsync(id);
            if (monthlyGoal == null)
            {
                result.SetError(MessageKey.MonthlyGoal_NotFound);
                return result;
            }

            result.SetResult(monthlyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<MonthlyGoalDto>> CreateAsync(CreateMonthlyGoalDto dto)
    {
        ServiceResult<MonthlyGoalDto> result = new();
        try
        {
            if (dto == null || dto.GoalId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            MonthlyGoal monthlyGoal = new MonthlyGoal(dto.GoalId, dto.Title, dto.Description, dto.DueDate);
            await monthlyGoalRepository.AddAsync(monthlyGoal);
            await monthlyGoalRepository.SaveChangesAsync();
            result.SetResult(monthlyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateMonthlyGoalDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            MonthlyGoal monthlyGoal = await monthlyGoalRepository.GetByIdAsync(dto.Id);
            if (monthlyGoal == null)
            {
                result.SetError(MessageKey.MonthlyGoal_NotFound);
                return result;
            }

            monthlyGoal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await monthlyGoalRepository.UpdateAsync(monthlyGoal);
            await monthlyGoalRepository.SaveChangesAsync();
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

            MonthlyGoal monthlyGoal = await monthlyGoalRepository.GetByIdAsync(id);
            if (monthlyGoal == null)
            {
                result.SetError(MessageKey.MonthlyGoal_NotFound);
                return result;
            }

            await monthlyGoalRepository.DeleteAsync(id);
            await monthlyGoalRepository.SaveChangesAsync();
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

            MonthlyGoal monthlyGoal = await monthlyGoalRepository.GetByIdAsync(id);
            if (monthlyGoal == null)
            {
                result.SetError(MessageKey.MonthlyGoal_NotFound);
                return result;
            }

            monthlyGoal.Update(monthlyGoal.Title, monthlyGoal.Description, monthlyGoal.DueDate, true);
            await monthlyGoalRepository.UpdateAsync(monthlyGoal);
            await monthlyGoalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }
}