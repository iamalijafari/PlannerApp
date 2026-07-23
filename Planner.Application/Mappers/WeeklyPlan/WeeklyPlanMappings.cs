using Planner.Application.DTOs.WeeklyPlan;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.WeeklyPlan;

public static class WeeklyPlanMappings
{
    public static IEnumerable<WeeklyPlanDto> ToDto(this IEnumerable<Domain.Entities.WeeklyPlan> weeklyPlans) =>
        weeklyPlans.Select(weeklyPlan => weeklyPlan.ToDto());

    public static WeeklyPlanDto ToDto(this Domain.Entities.WeeklyPlan weeklyPlan) =>
        new WeeklyPlanDto(
            weeklyPlan.Id,
            weeklyPlan.MonthlyPlanId,
            weeklyPlan.Title,
            weeklyPlan.Description,
            weeklyPlan.CreatedAt,
            weeklyPlan.DueDate,
            weeklyPlan.IsCompleted);
}