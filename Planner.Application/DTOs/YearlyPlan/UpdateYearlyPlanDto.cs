namespace Planner.Application.DTOs.YearlyPlan;

public record UpdateYearlyPlanDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);