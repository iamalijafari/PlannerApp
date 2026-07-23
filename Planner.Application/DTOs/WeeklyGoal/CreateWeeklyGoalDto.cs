namespace Planner.Application.DTOs.WeeklyGoal;

public record CreateWeeklyGoalDto(
    Guid MonthlyGoalId,
    string Title,
    string Description,
    DateTime DueDate);