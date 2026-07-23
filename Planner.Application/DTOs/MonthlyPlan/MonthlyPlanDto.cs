namespace Planner.Application.DTOs.MonthlyPlan;

public record MonthlyPlanDto(
    Guid Id,
    Guid YearlyPlanId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);