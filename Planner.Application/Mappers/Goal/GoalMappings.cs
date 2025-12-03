using Planner.Application.DTOs.Goal;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.Goal;

public static class GoalMappings
{
    public static IEnumerable<GoalDto> ToDto(this IEnumerable<Domain.Entities.Goal> goals) =>
        goals.Select(goal => goal.ToDto());

    public static GoalDto ToDto(this Domain.Entities.Goal goal) =>
        new GoalDto(
            goal.Id,
            goal.Title,
            goal.Description,
            goal.CreatedAt,
            goal.DueDate,
            goal.IsCompleted);
}