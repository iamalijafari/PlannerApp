using Planner.Application.Mappers.Goal;
using Planner.Domain.Entities;

namespace Planner.UnitTests.Application;

public class GoalTreeMappingsTests
{
    [Fact]
    public void ToTreeDto_OrdersEveryHierarchyLevelByDueDate()
    {
        Goal goal = new("Build a product", "Deliver it in small steps.", Utc(2028, 1, 1));
        YearlyPlan lateYear = new(goal.Id, "Year later", "", Utc(2027, 12, 31));
        YearlyPlan earlyYear = new(goal.Id, "Year earlier", "", Utc(2027, 1, 1));
        goal.YearlyPlans.Add(lateYear);
        goal.YearlyPlans.Add(earlyYear);

        MonthlyPlan lateMonth = new(earlyYear.Id, "Month later", "", Utc(2027, 3, 31));
        MonthlyPlan earlyMonth = new(earlyYear.Id, "Month earlier", "", Utc(2027, 2, 28));
        earlyYear.MonthlyPlans.Add(lateMonth);
        earlyYear.MonthlyPlans.Add(earlyMonth);

        WeeklyPlan lateWeek = new(earlyMonth.Id, "Week later", "", Utc(2027, 2, 14));
        WeeklyPlan earlyWeek = new(earlyMonth.Id, "Week earlier", "", Utc(2027, 2, 7));
        earlyMonth.WeeklyPlans.Add(lateWeek);
        earlyMonth.WeeklyPlans.Add(earlyWeek);

        DailyPlan lateDay = new(earlyWeek.Id, "Day later", "", Utc(2027, 2, 2));
        DailyPlan earlyDay = new(earlyWeek.Id, "Day earlier", "", Utc(2027, 2, 1));
        earlyWeek.DailyPlans.Add(lateDay);
        earlyWeek.DailyPlans.Add(earlyDay);

        var tree = goal.ToTreeDto();

        Assert.Equal(["Year earlier", "Year later"], tree.YearlyPlans.Select(plan => plan.Title));
        var mappedYear = tree.YearlyPlans.First();
        Assert.Equal(["Month earlier", "Month later"], mappedYear.MonthlyPlans.Select(plan => plan.Title));
        var mappedMonth = mappedYear.MonthlyPlans.First();
        Assert.Equal(["Week earlier", "Week later"], mappedMonth.WeeklyPlans.Select(plan => plan.Title));
        var mappedWeek = mappedMonth.WeeklyPlans.First();
        Assert.Equal(["Day earlier", "Day later"], mappedWeek.DailyPlans.Select(plan => plan.Title));
    }

    private static DateTime Utc(int year, int month, int day) =>
        new(year, month, day, 0, 0, 0, DateTimeKind.Utc);
}
