namespace Planner.Application.DTOs.MonthlyGoal;

public record UpdateMonthlyGoalDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);