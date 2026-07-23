namespace Planner.Api.DTOs.Requests.MonthlyPlan;

public class CreateMonthlyPlanRequestModel
{
    public Guid YearlyPlanId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}