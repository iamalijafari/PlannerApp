namespace Planner.Application.DTOs.Report;

public record GoalsProgressReportDto(
    int TotalGoals,
    int ActiveGoals,
    int CompletedGoals,
    int OverdueGoals,
    int CompletedLeafPlans,
    int TotalLeafPlans,
    int OverallProgressPercentage,
    IReadOnlyCollection<GoalProgressDto> Goals);
