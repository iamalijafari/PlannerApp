namespace Planner.Application.DTOs.DailyPlan;

public record UpdateDailyPlanDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);