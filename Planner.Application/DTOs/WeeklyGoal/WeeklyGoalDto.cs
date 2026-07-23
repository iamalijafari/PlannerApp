namespace Planner.Application.DTOs.WeeklyGoal;

public record WeeklyGoalDto(
    Guid Id,
    Guid MonthlyGoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);