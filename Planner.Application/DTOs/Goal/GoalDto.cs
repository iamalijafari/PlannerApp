namespace Planner.Application.DTOs.Goal;

public record GoalDto(
    Guid Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime DueDate,
    bool IsCompleted);