using Planner.Application.DTOs.YearlyGoal;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.YearlyGoal;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;

namespace Planner.Application.Services;

public class YearlyGoalService : IYearlyGoalService
{
    private readonly IYearlyGoalRepository yearlyGoalRepository;

    public YearlyGoalService(IYearlyGoalRepository yearlyGoalRepository)
    {
        this.yearlyGoalRepository = yearlyGoalRepository;
    }

    public async Task<ServiceResult<IEnumerable<YearlyGoalDto>>> GetAllByGoalIdAsync(Guid goalId)
    {
        ServiceResult<IEnumerable<YearlyGoalDto>> result = new();
        try
        {
            if (goalId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<YearlyGoal> yearlyGoals = await yearlyGoalRepository.GetAllByGoalIdAsync(goalId);
            result.SetResult(yearlyGoals.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<YearlyGoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<YearlyGoalDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(id);
            if (yearlyGoal == null)
            {
                result.SetError(MessageKey.YearlyGoal_NotFound);
                return result;
            }

            result.SetResult(yearlyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
        }
        return result;
    }

    public async Task<ServiceResult<YearlyGoalDto>> CreateAsync(CreateYearlyGoalDto dto)
    {
        ServiceResult<YearlyGoalDto> result = new();
        try
        {
            if (dto == null || dto.GoalId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            YearlyGoal yearlyGoal = new YearlyGoal(dto.GoalId, dto.Title, dto.Description, dto.DueDate);
            await yearlyGoalRepository.AddAsync(yearlyGoal);
            await yearlyGoalRepository.SaveChangesAsync();
            result.SetResult(yearlyGoal.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateYearlyGoalDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(dto.Id);
            if (yearlyGoal == null)
            {
                result.SetError(MessageKey.YearlyGoal_NotFound);
                return result;
            }

            yearlyGoal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await yearlyGoalRepository.UpdateAsync(yearlyGoal);
            await yearlyGoalRepository.SaveChangesAsync();
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

            YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(id);
            if (yearlyGoal == null)
            {
                result.SetError(MessageKey.YearlyGoal_NotFound);
                return result;
            }

            await yearlyGoalRepository.DeleteAsync(id);
            await yearlyGoalRepository.SaveChangesAsync();
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

            YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(id);
            if (yearlyGoal == null)
            {
                result.SetError(MessageKey.YearlyGoal_NotFound);
                return result;
            }

            yearlyGoal.Update(yearlyGoal.Title, yearlyGoal.Description, yearlyGoal.DueDate, true);
            await yearlyGoalRepository.UpdateAsync(yearlyGoal);
            await yearlyGoalRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
        }
        return result;
    }
}