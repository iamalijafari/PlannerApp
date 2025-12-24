namespace Planner.Api.DTOs.Requests.YearlyGoal;

public class CreateYearlyGoalRequestModel
{
    public Guid GoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}