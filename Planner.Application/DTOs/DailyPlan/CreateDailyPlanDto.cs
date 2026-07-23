namespace Planner.Application.DTOs.DailyPlan;

public record CreateDailyPlanDto(
    Guid WeeklyPlanId,
    string Title,
    string Description,
    DateTime DueDate);