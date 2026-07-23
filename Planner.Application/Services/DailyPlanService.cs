using Planner.Application.DTOs.DailyPlan;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.DailyPlan;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;
using Microsoft.Extensions.Logging;

namespace Planner.Application.Services;

public class DailyPlanService : IDailyPlanService
{
    private readonly IDailyPlanRepository dailyPlanRepository;
    private readonly ILogger<DailyPlanService> logger;

    public DailyPlanService
    (
        IDailyPlanRepository dailyPlanRepository,
        ILogger<DailyPlanService> logger
    )
    {
        this.dailyPlanRepository = dailyPlanRepository;
        this.logger = logger;
    }

    public async Task<ServiceResult<IEnumerable<DailyPlanDto>>> GetAllByWeeklyPlanIdAsync(Guid weeklyPlanId)
    {
        ServiceResult<IEnumerable<DailyPlanDto>> result = new();
        try
        {
            if (weeklyPlanId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<DailyPlan> dailyPlans = await dailyPlanRepository.GetAllByWeeklyPlanIdAsync(weeklyPlanId);
            result.SetResult(dailyPlans.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get all daily plans by weekly plan id");
        }
        return result;
    }

    public async Task<ServiceResult<DailyPlanDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<DailyPlanDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            DailyPlan dailyPlan = await dailyPlanRepository.GetByIdAsync(id);
            if (dailyPlan == null)
            {
                result.SetError(MessageKey.DailyPlan_NotFound);
                return result;
            }

            result.SetResult(dailyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get daily plan by id");
        }
        return result;
    }

    public async Task<ServiceResult<DailyPlanDto>> CreateAsync(CreateDailyPlanDto dto)
    {
        ServiceResult<DailyPlanDto> result = new();
        try
        {
            if (dto == null || dto.WeeklyPlanId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            DailyPlan dailyPlan = new DailyPlan(dto.WeeklyPlanId, dto.Title, dto.Description, dto.DueDate);
            await dailyPlanRepository.AddAsync(dailyPlan);
            await dailyPlanRepository.SaveChangesAsync();
            result.SetResult(dailyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to create daily plan");
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateDailyPlanDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            DailyPlan dailyPlan = await dailyPlanRepository.GetByIdAsync(dto.Id);
            if (dailyPlan == null)
            {
                result.SetError(MessageKey.DailyPlan_NotFound);
                return result;
            }

            dailyPlan.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await dailyPlanRepository.UpdateAsync(dailyPlan);
            await dailyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to update daily plan");
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

            DailyPlan dailyPlan = await dailyPlanRepository.GetByIdAsync(id);
            if (dailyPlan == null)
            {
                result.SetError(MessageKey.DailyPlan_NotFound);
                return result;
            }

            await dailyPlanRepository.DeleteAsync(id);
            await dailyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to delete daily plan");
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

            DailyPlan dailyPlan = await dailyPlanRepository.GetByIdAsync(id);
            if (dailyPlan == null)
            {
                result.SetError(MessageKey.DailyPlan_NotFound);
                return result;
            }

            dailyPlan.Update(dailyPlan.Title, dailyPlan.Description, dailyPlan.DueDate, true);
            await dailyPlanRepository.UpdateAsync(dailyPlan);
            await dailyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to complete daily plan");
        }
        return result;
    }
}