namespace Planner.Api.DTOs.Responses.Report;

public class GoalProgressResponseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public int CompletedLeafPlans { get; set; }
    public int TotalLeafPlans { get; set; }
    public int ProgressPercentage { get; set; }
    public string Status { get; set; } = string.Empty;
}
