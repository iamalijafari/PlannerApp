namespace Planner.Api.DTOs.Requests.WeeklyPlan;

public class CreateWeeklyPlanRequestModel
{
    public Guid MonthlyPlanId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}