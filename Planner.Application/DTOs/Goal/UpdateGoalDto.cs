namespace Planner.Application.DTOs.Goal;

public record UpdateGoalDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted);