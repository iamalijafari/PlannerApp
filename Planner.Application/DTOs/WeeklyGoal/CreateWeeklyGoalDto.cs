namespace Planner.Application.DTOs.WeeklyGoal;

public record CreateWeeklyGoalDto(
    Guid GoalId,
    string Title,
    string Description,
    DateTime DueDate);