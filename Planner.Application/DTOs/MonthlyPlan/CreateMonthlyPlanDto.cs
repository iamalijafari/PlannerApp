namespace Planner.Application.DTOs.MonthlyPlan;

public record CreateMonthlyPlanDto(
    Guid YearlyPlanId,
    string Title,
    string Description,
    DateTime DueDate);