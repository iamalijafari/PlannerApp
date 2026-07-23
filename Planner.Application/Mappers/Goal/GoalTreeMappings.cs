using Planner.Application.DTOs.Goal;

namespace Planner.Application.Mappers.Goal;

public static class GoalTreeMappings
{
    public static GoalTreeDto ToTreeDto(this Domain.Entities.Goal goal) =>
        new(goal.Id, goal.Title, goal.Description, goal.CreatedAt, goal.DueDate, goal.IsCompleted,
            goal.YearlyPlans.Select(y => y.ToTreeDto()));

    public static YearlyPlanTreeDto ToTreeDto(this Domain.Entities.YearlyPlan y) =>
        new(y.Id, y.Title, y.Description, y.CreatedAt, y.DueDate, y.IsCompleted,
            y.MonthlyPlans.Select(m => m.ToTreeDto()));

    public static MonthlyPlanTreeDto ToTreeDto(this Domain.Entities.MonthlyPlan m) =>
        new(m.Id, m.Title, m.Description, m.CreatedAt, m.DueDate, m.IsCompleted,
            m.WeeklyPlans.Select(w => w.ToTreeDto()));

    public static WeeklyPlanTreeDto ToTreeDto(this Domain.Entities.WeeklyPlan w) =>
        new(w.Id, w.Title, w.Description, w.CreatedAt, w.DueDate, w.IsCompleted,
            w.DailyPlans.Select(d => d.ToTreeDto()));

    public static DailyPlanTreeDto ToTreeDto(this Domain.Entities.DailyPlan d) =>
        new(d.Id, d.Title, d.Description, d.CreatedAt, d.DueDate, d.IsCompleted);
}