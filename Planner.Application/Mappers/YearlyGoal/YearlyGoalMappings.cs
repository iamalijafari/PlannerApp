using Planner.Application.DTOs.YearlyGoal;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.YearlyGoal;

public static class YearlyGoalMappings
{
    public static IEnumerable<YearlyGoalDto> ToDto(this IEnumerable<Domain.Entities.YearlyGoal> yearlyGoals) =>
        yearlyGoals.Select(yearlyGoal => yearlyGoal.ToDto());

    public static YearlyGoalDto ToDto(this Domain.Entities.YearlyGoal yearlyGoal) =>
        new YearlyGoalDto(
            yearlyGoal.Id,
            yearlyGoal.GoalId,
            yearlyGoal.Title,
            yearlyGoal.Description,
            yearlyGoal.CreatedAt,
            yearlyGoal.DueDate,
            yearlyGoal.IsCompleted);
}