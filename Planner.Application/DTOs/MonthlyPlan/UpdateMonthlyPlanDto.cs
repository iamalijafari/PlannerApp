namespace Planner.Application.DTOs.MonthlyPlan;

public record UpdateMonthlyPlanDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);