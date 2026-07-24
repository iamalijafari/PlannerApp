using Planner.Api.DTOs.Responses.Report;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Report;
using Planner.Application.DTOs.Utility;
using Planner.Application.Enumerations;

namespace Planner.Api.Mappers.Report;

public static class ReportMappings
{
    public static ResponseModel<GoalsProgressReportResponseModel> ToResponseModel(
        this ServiceResult<GoalsProgressReportDto> dto) =>
        new()
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    private static GoalsProgressReportResponseModel ToResponseModel(
        this GoalsProgressReportDto dto) =>
        new()
        {
            TotalGoals = dto.TotalGoals,
            ActiveGoals = dto.ActiveGoals,
            CompletedGoals = dto.CompletedGoals,
            OverdueGoals = dto.OverdueGoals,
            CompletedLeafPlans = dto.CompletedLeafPlans,
            TotalLeafPlans = dto.TotalLeafPlans,
            OverallProgressPercentage = dto.OverallProgressPercentage,
            Goals = dto.Goals.Select(goal => goal.ToResponseModel())
        };

    private static GoalProgressResponseModel ToResponseModel(
        this GoalProgressDto dto) =>
        new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted,
            CompletedLeafPlans = dto.CompletedLeafPlans,
            TotalLeafPlans = dto.TotalLeafPlans,
            ProgressPercentage = dto.ProgressPercentage,
            Status = dto.Status switch
            {
                GoalProgressStatus.InProgress => "in-progress",
                GoalProgressStatus.Completed => "completed",
                GoalProgressStatus.Overdue => "overdue",
                _ => "planned"
            }
        };
}
