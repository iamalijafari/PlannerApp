namespace Planner.Api.DTOs.Responses.MonthlyGoal;

public class MonthlyGoalResponseModel
{
    public Guid Id { get; set; }
    public Guid YearlyGoalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}