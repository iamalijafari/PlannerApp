namespace Planner.Application.DTOs.Goal;

public record DailyPlanTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted);

public record WeeklyPlanTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<DailyPlanTreeDto> DailyPlans);

public record MonthlyPlanTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<WeeklyPlanTreeDto> WeeklyPlans);

public record YearlyPlanTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<MonthlyPlanTreeDto> MonthlyPlans);

public record GoalTreeDto(
    Guid Id, string Title, string Description,
    DateTime CreatedAt, DateTime DueDate, bool IsCompleted,
    IEnumerable<YearlyPlanTreeDto> YearlyPlans);