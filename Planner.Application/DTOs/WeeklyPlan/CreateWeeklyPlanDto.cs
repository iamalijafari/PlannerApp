namespace Planner.Application.DTOs.WeeklyPlan;

public record CreateWeeklyPlanDto(
    Guid MonthlyPlanId,
    string Title,
    string Description,
    DateTime DueDate);