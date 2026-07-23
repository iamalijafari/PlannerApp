namespace Planner.Application.DTOs.DailyPlan;

public record DailyPlanDto(
    Guid Id,
    Guid WeeklyPlanId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);