using Planner.Application.DTOs.Goal;

namespace Planner.Application.Mappers.Goal;

public static class GoalTreeMappings
{
    public static GoalTreeDto ToTreeDto(this Domain.Entities.Goal goal) =>
        new(goal.Id, goal.Title, goal.Description, goal.CreatedAt, goal.DueDate, goal.IsCompleted,
            goal.YearlyGoals.Select(y => y.ToTreeDto()));

    public static YearlyGoalTreeDto ToTreeDto(this Domain.Entities.YearlyGoal y) =>
        new(y.Id, y.Title, y.Description, y.CreatedAt, y.DueDate, y.IsCompleted,
            y.MonthlyGoals.Select(m => m.ToTreeDto()));

    public static MonthlyGoalTreeDto ToTreeDto(this Domain.Entities.MonthlyGoal m) =>
        new(m.Id, m.Title, m.Description, m.CreatedAt, m.DueDate, m.IsCompleted,
            m.WeeklyGoals.Select(w => w.ToTreeDto()));

    public static WeeklyGoalTreeDto ToTreeDto(this Domain.Entities.WeeklyGoal w) =>
        new(w.Id, w.Title, w.Description, w.CreatedAt, w.DueDate, w.IsCompleted,
            w.DailyGoals.Select(d => d.ToTreeDto()));

    public static DailyGoalTreeDto ToTreeDto(this Domain.Entities.DailyGoal d) =>
        new(d.Id, d.Title, d.Description, d.CreatedAt, d.DueDate, d.IsCompleted);
}