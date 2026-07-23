namespace Planner.Application.DTOs.MonthlyGoal;

public record CreateMonthlyGoalDto(
    Guid YearlyGoalId,
    string Title,
    string Description,
    DateTime DueDate);