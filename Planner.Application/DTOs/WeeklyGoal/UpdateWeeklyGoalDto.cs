namespace Planner.Application.DTOs.WeeklyGoal;

public record UpdateWeeklyGoalDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);