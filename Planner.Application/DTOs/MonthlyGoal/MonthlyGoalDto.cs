namespace Planner.Application.DTOs.MonthlyGoal;

public record MonthlyGoalDto(
    Guid Id,
    Guid YearlyGoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);