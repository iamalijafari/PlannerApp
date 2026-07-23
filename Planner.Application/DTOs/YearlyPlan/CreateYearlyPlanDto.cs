namespace Planner.Application.DTOs.YearlyPlan;

public record CreateYearlyPlanDto(
    Guid GoalId,
    string Title,
    string Description,
    DateTime DueDate);