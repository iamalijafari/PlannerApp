namespace Planner.Application.DTOs.DailyGoal;

public record DailyGoalDto(
    Guid Id,
    Guid WeeklyGoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);