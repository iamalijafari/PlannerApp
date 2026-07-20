namespace Planner.Application.DTOs.Goal;

public record DailyGoalTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted);

public record WeeklyGoalTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<DailyGoalTreeDto> DailyGoals);

public record MonthlyGoalTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<WeeklyGoalTreeDto> WeeklyGoals);

public record YearlyGoalTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<MonthlyGoalTreeDto> MonthlyGoals);

public record GoalTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<YearlyGoalTreeDto> YearlyGoals);