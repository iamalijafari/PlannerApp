namespace Planner.Application.DTOs.YearlyGoal;

public record CreateYearlyGoalDto(
    Guid GoalId,
    string Title,
    string Description,
    DateTime DueDate);