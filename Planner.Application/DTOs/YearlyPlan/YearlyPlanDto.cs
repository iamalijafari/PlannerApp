namespace Planner.Application.DTOs.YearlyPlan;

public record YearlyPlanDto(
    Guid Id,
    Guid GoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);