namespace Planner.Application.DTOs.DailyGoal;

public record DailyGoalDto(
    Guid Id,
    Guid GoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);