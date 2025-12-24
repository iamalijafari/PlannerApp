using Planner.Api.DTOs.Responses.YearlyGoal;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.YearlyGoal;
using System.Collections.Generic;
using Planner.Api.DTOs.Requests.YearlyGoal;

namespace Planner.Api.Mappers.YearlyGoal;

public static class SubGoalMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<SubGoalResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<SubGoalDto>> dtos) =>
        new ResponseModel<IEnumerable<SubGoalResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<SubGoalResponseModel> ToResponseModel(this ServiceResult<SubGoalDto> dto) =>
        new ResponseModel<SubGoalResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<SubGoalResponseModel> ToResponseModel(this IEnumerable<SubGoalDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static SubGoalResponseModel ToResponseModel(this SubGoalDto dto) =>
        new SubGoalResponseModel
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
    public static CreateSubGoalDto ToDto(this CreateSubGoalRequestModel requestModel) =>
        new CreateSubGoalDto(
            requestModel.GoalId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}