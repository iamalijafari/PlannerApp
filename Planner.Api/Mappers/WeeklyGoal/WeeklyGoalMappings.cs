using Planner.Api.DTOs.Responses.WeeklyGoal;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.WeeklyGoal;
using System.Collections.Generic;
using System.Linq;
using Planner.Api.DTOs.Requests.WeeklyGoal;

namespace Planner.Api.Mappers.WeeklyGoal;

public static class WeeklyGoalMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<WeeklyGoalResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<WeeklyGoalDto>> dtos) =>
        new ResponseModel<IEnumerable<WeeklyGoalResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<WeeklyGoalResponseModel> ToResponseModel(this ServiceResult<WeeklyGoalDto> dto) =>
        new ResponseModel<WeeklyGoalResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<WeeklyGoalResponseModel> ToResponseModel(this IEnumerable<WeeklyGoalDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static WeeklyGoalResponseModel ToResponseModel(this WeeklyGoalDto dto) =>
        new WeeklyGoalResponseModel
        {
            Id = dto.Id,
            GoalId = dto.GoalId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateWeeklyGoalDto ToDto(this CreateWeeklyGoalRequestModel requestModel) =>
        new CreateWeeklyGoalDto(
            requestModel.GoalId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}