using Planner.Application.DTOs.YearlyGoal;
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

    public async Task<ServiceResult<IEnumerable<YearlyGoalDto>>> GetAllAsync()
    {
        ServiceResult<IEnumerable<YearlyGoalDto>> result = new();
        IEnumerable<YearlyGoal> yearlyGoals = await yearlyGoalRepository.GetAllAsync();
        result.SetResult(yearlyGoals.ToDto());
        return result;
    }

    public async Task<ServiceResult<YearlyGoalDto>> GetByIdAsync(Guid id)
    {
        ServiceResult<YearlyGoalDto> result = new();
        YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(id);
        result.SetResult(yearlyGoal.ToDto());
        return result;
    }

    public async Task<ServiceResult<YearlyGoalDto>> CreateAsync(CreateYearlyGoalDto dto)
    {
        ServiceResult<YearlyGoalDto> result = new();
        YearlyGoal yearlyGoal = new YearlyGoal(dto.GoalId, dto.Title, dto.Description, dto.DueDate);
        await yearlyGoalRepository.AddAsync(yearlyGoal);
        await yearlyGoalRepository.SaveChangesAsync();
        result.SetResult(yearlyGoal.ToDto());
        return result;
    }

    public async Task<ServiceResult<bool>> UpdateAsync(UpdateYearlyGoalDto dto)
    {
        ServiceResult<bool> result = new();
        YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(dto.Id);
        yearlyGoal.Update(dto.Title, dto.Description, dto.DueDate, dto.IsCompleted);
        await yearlyGoalRepository.UpdateAsync(yearlyGoal);
        await yearlyGoalRepository.SaveChangesAsync();
        result.SetResult(true);
        return result;
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
    {
        ServiceResult<bool> result = new();
        await yearlyGoalRepository.DeleteAsync(id);
        await yearlyGoalRepository.SaveChangesAsync();
        result.SetResult(true);
        return result;
    }

    public async Task<ServiceResult<bool>> CompleteAsync(Guid id)
    {
        ServiceResult<bool> result = new();
        YearlyGoal yearlyGoal = await yearlyGoalRepository.GetByIdAsync(id);
        yearlyGoal.Update(yearlyGoal.Title, yearlyGoal.Description, yearlyGoal.DueDate, true);
        await yearlyGoalRepository.UpdateAsync(yearlyGoal);
        await yearlyGoalRepository.SaveChangesAsync();
        result.SetResult(true);
        return result;
    }
}