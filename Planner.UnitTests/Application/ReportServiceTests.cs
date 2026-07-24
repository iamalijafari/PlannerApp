using Microsoft.Extensions.Logging;
using NSubstitute;
using Planner.Application.Enumerations;
using Planner.Application.Interfaces.Repositories;
using Planner.Application.Services;
using Planner.Domain.Entities;

namespace Planner.UnitTests.Application;

public class ReportServiceTests
{
    private static readonly DateTime ReportTime =
        new(2026, 7, 24, 12, 0, 0, DateTimeKind.Utc);

    private readonly IGoalRepository goalRepository =
        Substitute.For<IGoalRepository>();

    private readonly ILogger<ReportService> logger =
        Substitute.For<ILogger<ReportService>>();

    [Fact]
    public async Task GetGoalsProgressAsync_CountsOnlyDeepestPlans()
    {
        Goal goal = BuildGoalWithMixedLeafLevels();
        goalRepository.GetAllWithPlansAsync()
            .Returns(Task.FromResult<IEnumerable<Goal>>([goal]));

        ReportService service = new(
            goalRepository,
            logger,
            new FixedTimeProvider(ReportTime));

        var result = await service.GetGoalsProgressAsync();

        Assert.True(result.Success);
        Assert.NotNull(result.Result);

        var goalReport = Assert.Single(result.Result.Goals);
        Assert.Equal(5, goalReport.TotalLeafPlans);
        Assert.Equal(3, goalReport.CompletedLeafPlans);
        Assert.Equal(60, goalReport.ProgressPercentage);
        Assert.Equal(GoalProgressStatus.InProgress, goalReport.Status);
        Assert.Equal(5, result.Result.TotalLeafPlans);
        Assert.Equal(3, result.Result.CompletedLeafPlans);
        Assert.Equal(60, result.Result.OverallProgressPercentage);
    }

    [Fact]
    public async Task GetGoalsProgressAsync_BuildsPartitionedGoalSummary()
    {
        Goal activeGoal = BuildGoalWithMixedLeafLevels();
        Goal plannedGoal = new(
            "Plan a conference talk",
            "",
            ReportTime.AddMonths(2));
        Goal overdueGoal = new(
            "Renew a certification",
            "",
            ReportTime.AddDays(-1));
        Goal completedGoal = new(
            "Publish the portfolio",
            "",
            ReportTime.AddDays(-10));
        completedGoal.MarkAsCompleted();

        goalRepository.GetAllWithPlansAsync()
            .Returns(Task.FromResult<IEnumerable<Goal>>(
                [activeGoal, plannedGoal, overdueGoal, completedGoal]));

        ReportService service = new(
            goalRepository,
            logger,
            new FixedTimeProvider(ReportTime));

        var result = await service.GetGoalsProgressAsync();

        Assert.True(result.Success);
        Assert.NotNull(result.Result);
        Assert.Equal(4, result.Result.TotalGoals);
        Assert.Equal(2, result.Result.ActiveGoals);
        Assert.Equal(1, result.Result.CompletedGoals);
        Assert.Equal(1, result.Result.OverdueGoals);
    }

    [Fact]
    public async Task GetGoalsProgressAsync_WhenRepositoryFails_ReturnsServerError()
    {
        goalRepository.GetAllWithPlansAsync().Returns(
            Task.FromException<IEnumerable<Goal>>(
                new InvalidOperationException("Database unavailable")));

        ReportService service = new(
            goalRepository,
            logger,
            new FixedTimeProvider(ReportTime));

        var result = await service.GetGoalsProgressAsync();

        Assert.False(result.Success);
        Assert.Equal(MessageKey.ServerError, result.MessageKey);
    }

    private static Goal BuildGoalWithMixedLeafLevels()
    {
        Goal goal = new(
            "Move into a senior engineering role",
            "Build the skills and portfolio for the next role.",
            ReportTime.AddMonths(6));

        YearlyPlan yearlyWithChildren = new(
            goal.Id,
            "Build professional momentum",
            "",
            ReportTime.AddMonths(5));
        goal.YearlyPlans.Add(yearlyWithChildren);

        MonthlyPlan monthlyWithChildren = new(
            yearlyWithChildren.Id,
            "Ship portfolio work",
            "",
            ReportTime.AddMonths(2));
        yearlyWithChildren.MonthlyPlans.Add(monthlyWithChildren);

        WeeklyPlan weeklyWithDailyPlans = new(
            monthlyWithChildren.Id,
            "Finish the case study",
            "",
            ReportTime.AddDays(14));
        monthlyWithChildren.WeeklyPlans.Add(weeklyWithDailyPlans);

        DailyPlan completedDailyPlan = new(
            weeklyWithDailyPlans.Id,
            "Write the architecture section",
            "",
            ReportTime.AddDays(2));
        completedDailyPlan.MarkAsCompleted();
        weeklyWithDailyPlans.DailyPlans.Add(completedDailyPlan);

        DailyPlan pendingDailyPlan = new(
            weeklyWithDailyPlans.Id,
            "Add final screenshots",
            "",
            ReportTime.AddDays(3));
        weeklyWithDailyPlans.DailyPlans.Add(pendingDailyPlan);

        WeeklyPlan weeklyLeaf = new(
            monthlyWithChildren.Id,
            "Prepare the release",
            "",
            ReportTime.AddDays(21));
        weeklyLeaf.MarkAsCompleted();
        monthlyWithChildren.WeeklyPlans.Add(weeklyLeaf);

        MonthlyPlan monthlyLeaf = new(
            yearlyWithChildren.Id,
            "Practice interviews",
            "",
            ReportTime.AddMonths(3));
        yearlyWithChildren.MonthlyPlans.Add(monthlyLeaf);

        YearlyPlan yearlyLeaf = new(
            goal.Id,
            "Grow the professional network",
            "",
            ReportTime.AddMonths(4));
        yearlyLeaf.MarkAsCompleted();
        goal.YearlyPlans.Add(yearlyLeaf);

        return goal;
    }

    private sealed class FixedTimeProvider(DateTime utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(utcNow);
    }
}
