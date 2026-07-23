namespace Planner.Api.DTOs.Responses.WeeklyGoal;

public class WeeklyGoalResponseModel
{
    public Guid Id { get; set; }
    public Guid MonthlyGoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}