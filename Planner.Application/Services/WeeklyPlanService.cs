using Planner.Application.DTOs.WeeklyPlan;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.WeeklyPlan;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;
using Microsoft.Extensions.Logging;

namespace Planner.Application.Services;

public class WeeklyPlanService : IWeeklyPlanService
{
    private readonly IWeeklyPlanRepository weeklyPlanRepository;
    private readonly ILogger<WeeklyPlanService> logger;

    public WeeklyPlanService
    (
        IWeeklyPlanRepository weeklyPlanRepository,
        ILogger<WeeklyPlanService> logger
    )
    {
        this.weeklyPlanRepository = weeklyPlanRepository;
        this.logger = logger;
    }

    public async Task<ServiceResult<IEnumerable<WeeklyPlanDto>>> GetAllByMonthlyPlanIdAsync(Guid monthlyPlanId)
    {
        ServiceResult<IEnumerable<WeeklyPlanDto>> result = new();
        try
        {
            if (monthlyPlanId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<WeeklyPlan> weeklyPlans = await weeklyPlanRepository.GetAllByMonthlyPlanIdAsync(monthlyPlanId);
            result.SetResult(weeklyPlans.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get all weekly plans by monthly plan id");
        }
        return result;
    }

    public async Task<ServiceResult<WeeklyPlanDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<WeeklyPlanDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            WeeklyPlan weeklyPlan = await weeklyPlanRepository.GetByIdAsync(id);
            if (weeklyPlan == null)
            {
                result.SetError(MessageKey.WeeklyPlan_NotFound);
                return result;
            }

            result.SetResult(weeklyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get weekly plan by id");
        }
        return result;
    }

    public async Task<ServiceResult<WeeklyPlanDto>> CreateAsync(CreateWeeklyPlanDto dto)
    {
        ServiceResult<WeeklyPlanDto> result = new();
        try
        {
            if (dto == null || dto.MonthlyPlanId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            WeeklyPlan weeklyPlan = new WeeklyPlan(dto.MonthlyPlanId, dto.Title, dto.Description, dto.DueDate);
            await weeklyPlanRepository.AddAsync(weeklyPlan);
            await weeklyPlanRepository.SaveChangesAsync();
            result.SetResult(weeklyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to create weekly plan");
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateWeeklyPlanDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            WeeklyPlan weeklyPlan = await weeklyPlanRepository.GetByIdAsync(dto.Id);
            if (weeklyPlan == null)
            {
                result.SetError(MessageKey.WeeklyPlan_NotFound);
                return result;
            }

            weeklyPlan.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await weeklyPlanRepository.UpdateAsync(weeklyPlan);
            await weeklyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to update weekly plan");
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

            WeeklyPlan weeklyPlan = await weeklyPlanRepository.GetByIdAsync(id);
            if (weeklyPlan == null)
            {
                result.SetError(MessageKey.WeeklyPlan_NotFound);
                return result;
            }

            await weeklyPlanRepository.DeleteAsync(id);
            await weeklyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to delete weekly plan");
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

            WeeklyPlan weeklyPlan = await weeklyPlanRepository.GetByIdAsync(id);
            if (weeklyPlan == null)
            {
                result.SetError(MessageKey.WeeklyPlan_NotFound);
                return result;
            }

            weeklyPlan.Update(weeklyPlan.Title, weeklyPlan.Description, weeklyPlan.DueDate, true);
            await weeklyPlanRepository.UpdateAsync(weeklyPlan);
            await weeklyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to complete weekly plan");
        }
        return result;
    }
}