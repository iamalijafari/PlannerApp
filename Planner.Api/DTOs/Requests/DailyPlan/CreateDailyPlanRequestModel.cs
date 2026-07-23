namespace Planner.Api.DTOs.Requests.DailyPlan;

public class CreateDailyPlanRequestModel
{
    public Guid WeeklyPlanId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}