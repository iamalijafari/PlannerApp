using Planner.Application.DTOs.WeeklyGoal;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.WeeklyGoal;

public static class WeeklyGoalMappings
{
    public static IEnumerable<WeeklyGoalDto> ToDto(this IEnumerable<Domain.Entities.WeeklyGoal> weeklyGoals) =>
        weeklyGoals.Select(weeklyGoal => weeklyGoal.ToDto());

    public static WeeklyGoalDto ToDto(this Domain.Entities.WeeklyGoal weeklyGoal) =>
        new WeeklyGoalDto(
            weeklyGoal.Id,
            weeklyGoal.MonthlyGoalId,
            weeklyGoal.Title,
            weeklyGoal.Description,
            weeklyGoal.CreatedAt,
            weeklyGoal.DueDate,
            weeklyGoal.IsCompleted);
}