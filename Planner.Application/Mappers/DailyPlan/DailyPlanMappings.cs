using Planner.Application.DTOs.DailyPlan;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.DailyPlan;

public static class DailyPlanMappings
{
    public static IEnumerable<DailyPlanDto> ToDto(this IEnumerable<Domain.Entities.DailyPlan> dailyPlans) =>
        dailyPlans.Select(dailyPlan => dailyPlan.ToDto());

    public static DailyPlanDto ToDto(this Domain.Entities.DailyPlan dailyPlan) =>
        new DailyPlanDto(
            dailyPlan.Id,
            dailyPlan.WeeklyPlanId,
            dailyPlan.Title,
            dailyPlan.Description,
            dailyPlan.CreatedAt,
            dailyPlan.DueDate,
            dailyPlan.IsCompleted);
}