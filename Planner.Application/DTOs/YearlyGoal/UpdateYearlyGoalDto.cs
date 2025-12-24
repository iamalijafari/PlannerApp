namespace Planner.Application.DTOs.YearlyGoal;

public record UpdateYearlyGoalDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);