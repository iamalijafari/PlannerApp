using Planner.Application.DTOs.YearlyPlan;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.YearlyPlan;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;
using Microsoft.Extensions.Logging;

namespace Planner.Application.Services;

public class YearlyPlanService : IYearlyPlanService
{
    private readonly IYearlyPlanRepository yearlyPlanRepository;
    private readonly ILogger<YearlyPlanService> logger;

    public YearlyPlanService
    (
        IYearlyPlanRepository yearlyPlanRepository,
        ILogger<YearlyPlanService> logger
    )
    {
        this.yearlyPlanRepository = yearlyPlanRepository;
        this.logger = logger;
    }

    public async Task<ServiceResult<IEnumerable<YearlyPlanDto>>> GetAllByGoalIdAsync(Guid goalId)
    {
        ServiceResult<IEnumerable<YearlyPlanDto>> result = new();
        try
        {
            if (goalId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<YearlyPlan> yearlyPlans = await yearlyPlanRepository.GetAllByGoalIdAsync(goalId);
            result.SetResult(yearlyPlans.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get all yearly plans by goal id");
        }
        return result;
    }

    public async Task<ServiceResult<YearlyPlanDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<YearlyPlanDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            YearlyPlan yearlyPlan = await yearlyPlanRepository.GetByIdAsync(id);
            if (yearlyPlan == null)
            {
                result.SetError(MessageKey.YearlyPlan_NotFound);
                return result;
            }

            result.SetResult(yearlyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get yearly plan by id");
        }
        return result;
    }

    public async Task<ServiceResult<YearlyPlanDto>> CreateAsync(CreateYearlyPlanDto dto)
    {
        ServiceResult<YearlyPlanDto> result = new();
        try
        {
            if (dto == null || dto.GoalId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            YearlyPlan yearlyPlan = new YearlyPlan(dto.GoalId, dto.Title, dto.Description, dto.DueDate);
            await yearlyPlanRepository.AddAsync(yearlyPlan);
            await yearlyPlanRepository.SaveChangesAsync();
            result.SetResult(yearlyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to create yearly plan");
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateYearlyPlanDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            YearlyPlan yearlyPlan = await yearlyPlanRepository.GetByIdAsync(dto.Id);
            if (yearlyPlan == null)
            {
                result.SetError(MessageKey.YearlyPlan_NotFound);
                return result;
            }

            yearlyPlan.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await yearlyPlanRepository.UpdateAsync(yearlyPlan);
            await yearlyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to update yearly plan");
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

            YearlyPlan yearlyPlan = await yearlyPlanRepository.GetByIdAsync(id);
            if (yearlyPlan == null)
            {
                result.SetError(MessageKey.YearlyPlan_NotFound);
                return result;
            }

            await yearlyPlanRepository.DeleteAsync(id);
            await yearlyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to delete yearly plan");
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

            YearlyPlan yearlyPlan = await yearlyPlanRepository.GetByIdAsync(id);
            if (yearlyPlan == null)
            {
                result.SetError(MessageKey.YearlyPlan_NotFound);
                return result;
            }

            yearlyPlan.Update(yearlyPlan.Title, yearlyPlan.Description, yearlyPlan.DueDate, true);
            await yearlyPlanRepository.UpdateAsync(yearlyPlan);
            await yearlyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to complete yearly plan");
        }
        return result;
    }
}