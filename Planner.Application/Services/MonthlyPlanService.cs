using Planner.Application.DTOs.MonthlyPlan;
using Planner.Application.Interfaces.Services;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Mappers.MonthlyPlan;
using Planner.Domain.Entities;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;
using Microsoft.Extensions.Logging;

namespace Planner.Application.Services;

public class MonthlyPlanService : IMonthlyPlanService
{
    private readonly IMonthlyPlanRepository monthlyPlanRepository;
    private readonly ILogger<MonthlyPlanService> logger;

    public MonthlyPlanService
    (
        IMonthlyPlanRepository monthlyPlanRepository,
        ILogger<MonthlyPlanService> logger
    )
    {
        this.monthlyPlanRepository = monthlyPlanRepository;
        this.logger = logger;
    }

    public async Task<ServiceResult<IEnumerable<MonthlyPlanDto>>> GetAllByYearlyPlanIdAsync(Guid yearlyPlanId)
    {
        ServiceResult<IEnumerable<MonthlyPlanDto>> result = new();
        try
        {
            if (yearlyPlanId == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            IEnumerable<MonthlyPlan> monthlyPlans = await monthlyPlanRepository.GetAllByYearlyPlanIdAsync(yearlyPlanId);
            result.SetResult(monthlyPlans.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get all monthly plans by yearly plan id");
        }
        return result;
    }

    public async Task<ServiceResult<MonthlyPlanDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<MonthlyPlanDto> result = new();
        try
        {
            if (id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            MonthlyPlan monthlyPlan = await monthlyPlanRepository.GetByIdAsync(id);
            if (monthlyPlan == null)
            {
                result.SetError(MessageKey.MonthlyPlan_NotFound);
                return result;
            }

            result.SetResult(monthlyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to get monthly plan by id");
        }
        return result;
    }

    public async Task<ServiceResult<MonthlyPlanDto>> CreateAsync(CreateMonthlyPlanDto dto)
    {
        ServiceResult<MonthlyPlanDto> result = new();
        try
        {
            if (dto == null || dto.YearlyPlanId == Guid.Empty || string.IsNullOrWhiteSpace(dto.Title))
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            MonthlyPlan monthlyPlan = new MonthlyPlan(dto.YearlyPlanId, dto.Title, dto.Description, dto.DueDate);
            await monthlyPlanRepository.AddAsync(monthlyPlan);
            await monthlyPlanRepository.SaveChangesAsync();
            result.SetResult(monthlyPlan.ToDto());
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to create monthly plan");
        }
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateMonthlyPlanDto dto)
    {
        ServiceResult<bool> result = new();
        try
        {
            if (dto == null || dto.Id == Guid.Empty)
            {
                result.SetError(MessageKey.Invalid_Input);
                return result;
            }

            MonthlyPlan monthlyPlan = await monthlyPlanRepository.GetByIdAsync(dto.Id);
            if (monthlyPlan == null)
            {
                result.SetError(MessageKey.MonthlyPlan_NotFound);
                return result;
            }

            monthlyPlan.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
            await monthlyPlanRepository.UpdateAsync(monthlyPlan);
            await monthlyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to update monthly plan");
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

            MonthlyPlan monthlyPlan = await monthlyPlanRepository.GetByIdAsync(id);
            if (monthlyPlan == null)
            {
                result.SetError(MessageKey.MonthlyPlan_NotFound);
                return result;
            }

            await monthlyPlanRepository.DeleteAsync(id);
            await monthlyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to delete monthly plan");
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

            MonthlyPlan monthlyPlan = await monthlyPlanRepository.GetByIdAsync(id);
            if (monthlyPlan == null)
            {
                result.SetError(MessageKey.MonthlyPlan_NotFound);
                return result;
            }

            monthlyPlan.Update(monthlyPlan.Title, monthlyPlan.Description, monthlyPlan.DueDate, true);
            await monthlyPlanRepository.UpdateAsync(monthlyPlan);
            await monthlyPlanRepository.SaveChangesAsync();
            result.SetResult(true);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.Operation_Failed);
            logger.LogError(ex, "Failed to complete monthly plan");
        }
        return result;
    }
}