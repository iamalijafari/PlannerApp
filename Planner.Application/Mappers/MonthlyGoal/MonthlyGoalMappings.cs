using Planner.Application.DTOs.MonthlyGoal;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.MonthlyGoal;

public static class MonthlyGoalMappings
{
    public static IEnumerable<MonthlyGoalDto> ToDto(this IEnumerable<Domain.Entities.MonthlyGoal> monthlyGoals) =>
        monthlyGoals.Select(monthlyGoal => monthlyGoal.ToDto());

    public static MonthlyGoalDto ToDto(this Domain.Entities.MonthlyGoal monthlyGoal) =>
        new MonthlyGoalDto(
            monthlyGoal.Id,
            monthlyGoal.YearlyGoalId,
            monthlyGoal.Title,
            monthlyGoal.Description,
            monthlyGoal.CreatedAt,
            monthlyGoal.DueDate,
            monthlyGoal.IsCompleted);
}