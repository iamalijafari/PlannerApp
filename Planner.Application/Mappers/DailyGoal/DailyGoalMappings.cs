using Planner.Application.DTOs.DailyGoal;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.DailyGoal;

public static class DailyGoalMappings
{
    public static IEnumerable<DailyGoalDto> ToDto(this IEnumerable<Domain.Entities.DailyGoal> dailyGoals) =>
        dailyGoals.Select(dailyGoal => dailyGoal.ToDto());

    public static DailyGoalDto ToDto(this Domain.Entities.DailyGoal dailyGoal) =>
        new DailyGoalDto(
            dailyGoal.Id,
            dailyGoal.WeeklyGoalId,
            dailyGoal.Title,
            dailyGoal.Description,
            dailyGoal.CreatedAt,
            dailyGoal.DueDate,
            dailyGoal.IsCompleted);
}