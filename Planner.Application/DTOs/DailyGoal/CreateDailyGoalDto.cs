namespace Planner.Application.DTOs.DailyGoal;

public record CreateDailyGoalDto(
    Guid WeeklyGoalId,
    string Title,
    string Description,
    DateTime DueDate);