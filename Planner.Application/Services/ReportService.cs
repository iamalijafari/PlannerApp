using Microsoft.Extensions.Logging;
using Planner.Application.DTOs.Report;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Interfaces.Services;
using Planner.Domain.Entities;

namespace Planner.Application.Services;

public class ReportService : IReportService
{
    private readonly IGoalRepository goalRepository;
    private readonly ILogger<ReportService> logger;
    private readonly TimeProvider timeProvider;

    public ReportService(
        IGoalRepository goalRepository,
        ILogger<ReportService> logger,
        TimeProvider timeProvider)
    {
        this.goalRepository = goalRepository;
        this.logger = logger;
        this.timeProvider = timeProvider;
    }

    public async Task<ServiceResult<GoalsProgressReportDto>> GetGoalsProgressAsync()
    {
        ServiceResult<GoalsProgressReportDto> result = new();

        try
        {
            IEnumerable<Goal> goals = await goalRepository.GetAllWithPlansAsync();
            DateTime utcNow = timeProvider.GetUtcNow().UtcDateTime;

            List<GoalProgressDto> goalReports = goals
                .Select(goal => BuildGoalProgress(goal, utcNow))
                .ToList();

            int totalLeafPlans = goalReports.Sum(goal => goal.TotalLeafPlans);
            int completedLeafPlans = goalReports.Sum(goal => goal.CompletedLeafPlans);

            GoalsProgressReportDto report = new(
                TotalGoals: goalReports.Count,
                ActiveGoals: goalReports.Count(goal =>
                    goal.Status is GoalProgressStatus.Planned or GoalProgressStatus.InProgress),
                CompletedGoals: goalReports.Count(goal =>
                    goal.Status == GoalProgressStatus.Completed),
                OverdueGoals: goalReports.Count(goal =>
                    goal.Status == GoalProgressStatus.Overdue),
                CompletedLeafPlans: completedLeafPlans,
                TotalLeafPlans: totalLeafPlans,
                OverallProgressPercentage: CalculatePercentage(
                    completedLeafPlans,
                    totalLeafPlans),
                Goals: goalReports);

            result.SetResult(report);
        }
        catch (Exception ex)
        {
            result.SetError(MessageKey.ServerError);
            logger.LogError(ex, "Failed to build the goals progress report");
        }

        return result;
    }

    private static GoalProgressDto BuildGoalProgress(Goal goal, DateTime utcNow)
    {
        IReadOnlyCollection<bool> leafCompletionStates = GetLeafCompletionStates(goal);
        int totalLeafPlans = leafCompletionStates.Count;
        int completedLeafPlans = leafCompletionStates.Count(isCompleted => isCompleted);
        int progressPercentage = CalculatePercentage(completedLeafPlans, totalLeafPlans);

        GoalProgressStatus status = GetStatus(
            goal,
            totalLeafPlans,
            completedLeafPlans,
            utcNow);

        return new GoalProgressDto(
            goal.Id,
            goal.Title,
            goal.Description,
            goal.DueDate,
            goal.IsCompleted,
            completedLeafPlans,
            totalLeafPlans,
            progressPercentage,
            status);
    }

    private static IReadOnlyCollection<bool> GetLeafCompletionStates(Goal goal)
    {
        List<bool> leaves = [];

        foreach (YearlyPlan yearlyPlan in goal.YearlyPlans)
        {
            if (yearlyPlan.MonthlyPlans.Count == 0)
            {
                leaves.Add(yearlyPlan.IsCompleted);
                continue;
            }

            foreach (MonthlyPlan monthlyPlan in yearlyPlan.MonthlyPlans)
            {
                if (monthlyPlan.WeeklyPlans.Count == 0)
                {
                    leaves.Add(monthlyPlan.IsCompleted);
                    continue;
                }

                foreach (WeeklyPlan weeklyPlan in monthlyPlan.WeeklyPlans)
                {
                    if (weeklyPlan.DailyPlans.Count == 0)
                    {
                        leaves.Add(weeklyPlan.IsCompleted);
                        continue;
                    }

                    leaves.AddRange(weeklyPlan.DailyPlans.Select(plan => plan.IsCompleted));
                }
            }
        }

        return leaves;
    }

    private static GoalProgressStatus GetStatus(
        Goal goal,
        int totalLeafPlans,
        int completedLeafPlans,
        DateTime utcNow)
    {
        if (goal.IsCompleted ||
            (totalLeafPlans > 0 && completedLeafPlans == totalLeafPlans))
        {
            return GoalProgressStatus.Completed;
        }

        if (goal.DueDate < utcNow)
        {
            return GoalProgressStatus.Overdue;
        }

        return completedLeafPlans > 0
            ? GoalProgressStatus.InProgress
            : GoalProgressStatus.Planned;
    }

    private static int CalculatePercentage(int completed, int total)
    {
        if (total == 0)
        {
            return 0;
        }

        return (int)Math.Round(
            completed * 100d / total,
            MidpointRounding.AwayFromZero);
    }
}
