using Planner.Application.DTOs.YearlyPlan;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.YearlyPlan;

public static class YearlyPlanMappings
{
    public static IEnumerable<YearlyPlanDto> ToDto(this IEnumerable<Domain.Entities.YearlyPlan> yearlyPlans) =>
        yearlyPlans.Select(yearlyPlan => yearlyPlan.ToDto());

    public static YearlyPlanDto ToDto(this Domain.Entities.YearlyPlan yearlyPlan) =>
        new YearlyPlanDto(
            yearlyPlan.Id,
            yearlyPlan.GoalId,
            yearlyPlan.Title,
            yearlyPlan.Description,
            yearlyPlan.CreatedAt,
            yearlyPlan.DueDate,
            yearlyPlan.IsCompleted);
}