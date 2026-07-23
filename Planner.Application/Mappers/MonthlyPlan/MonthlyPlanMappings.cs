using Planner.Application.DTOs.MonthlyPlan;
using System.Collections.Generic;
using System.Linq;

namespace Planner.Application.Mappers.MonthlyPlan;

public static class MonthlyPlanMappings
{
    public static IEnumerable<MonthlyPlanDto> ToDto(this IEnumerable<Domain.Entities.MonthlyPlan> monthlyPlans) =>
        monthlyPlans.Select(monthlyPlan => monthlyPlan.ToDto());

    public static MonthlyPlanDto ToDto(this Domain.Entities.MonthlyPlan monthlyPlan) =>
        new MonthlyPlanDto(
            monthlyPlan.Id,
            monthlyPlan.YearlyPlanId,
            monthlyPlan.Title,
            monthlyPlan.Description,
            monthlyPlan.CreatedAt,
            monthlyPlan.DueDate,
            monthlyPlan.IsCompleted);
}