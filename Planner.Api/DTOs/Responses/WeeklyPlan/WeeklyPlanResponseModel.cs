namespace Planner.Api.DTOs.Responses.WeeklyPlan;

public class WeeklyPlanResponseModel
{
    public Guid Id { get; set; }
    public Guid MonthlyPlanId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}