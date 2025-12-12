namespace Planner.Api.DTOs.Requests.Goal;

public class CreateGoalRequestModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}