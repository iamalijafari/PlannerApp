namespace Planner.Application.DTOs.YearlyGoal;

public record YearlyGoalDto(
    Guid Id,
    Guid GoalId,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);