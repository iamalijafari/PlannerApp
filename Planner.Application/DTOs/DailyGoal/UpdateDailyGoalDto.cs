namespace Planner.Application.DTOs.DailyGoal;

public record UpdateDailyGoalDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);