namespace Planner.Application.DTOs.MonthlyGoal;

public record CreateMonthlyGoalDto(
    Guid GoalId,
    string Title,
    string Description,
    DateTime DueDate);