namespace Planner.Api.DTOs.Requests.WeeklyGoal;

public class CreateWeeklyGoalRequestModel
{
    public Guid MonthlyGoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}