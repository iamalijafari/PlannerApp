namespace Planner.Api.DTOs.Requests.YearlyPlan;

public class CreateYearlyPlanRequestModel
{
    public Guid GoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}