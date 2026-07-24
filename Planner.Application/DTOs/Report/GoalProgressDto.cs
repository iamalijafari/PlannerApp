using Planner.Application.Enumerations;

namespace Planner.Application.DTOs.Report;

public record GoalProgressDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    bool IsCompleted,
    int CompletedLeafPlans,
    int TotalLeafPlans,
    int ProgressPercentage,
    GoalProgressStatus Status);
