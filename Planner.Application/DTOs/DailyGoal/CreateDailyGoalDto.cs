namespace Planner.Application.DTOs.DailyGoal;

public record CreateDailyGoalDto(
    Guid GoalId,
    string Title,
    string Description,
    DateTime DueDate);