namespace Planner.Application.DTOs.WeeklyGoal;

public record WeeklyGoalDto(
    Guid Id,
    Guid GoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);