namespace Planner.Api.DTOs.Responses.Report;

public class GoalsProgressReportResponseModel
{
    public int TotalGoals { get; set; }
    public int ActiveGoals { get; set; }
    public int CompletedGoals { get; set; }
    public int OverdueGoals { get; set; }
    public int CompletedLeafPlans { get; set; }
    public int TotalLeafPlans { get; set; }
    public int OverallProgressPercentage { get; set; }
    public IEnumerable<GoalProgressResponseModel> Goals { get; set; } = [];
}
