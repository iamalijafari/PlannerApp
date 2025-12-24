namespace Planner.Api.DTOs.Responses.YearlyGoal;

public class SubGoalResponseModel
{
    public Guid Id { get; set; }
    public Guid GoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}