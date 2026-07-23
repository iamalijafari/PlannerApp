namespace Planner.Application.DTOs.WeeklyPlan;

public record WeeklyPlanDto(
    Guid Id,
    Guid MonthlyPlanId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);