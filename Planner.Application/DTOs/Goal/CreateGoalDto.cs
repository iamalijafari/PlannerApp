namespace Planner.Application.DTOs.Goal;

public record CreateGoalDto(
    string Title,
    string Description,
    DateTime DueDate);