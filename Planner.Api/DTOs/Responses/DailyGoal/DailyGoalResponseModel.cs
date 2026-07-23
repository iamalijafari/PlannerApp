namespace Planner.Api.DTOs.Responses.DailyGoal;

public class DailyGoalResponseModel
{
    public Guid Id { get; set; }
    public Guid WeeklyGoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}