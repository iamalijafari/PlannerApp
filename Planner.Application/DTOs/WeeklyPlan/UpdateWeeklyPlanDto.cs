namespace Planner.Application.DTOs.WeeklyPlan;

public record UpdateWeeklyPlanDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);