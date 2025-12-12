using Planner.Api.DTOs.Responses.Goal;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.Goal;
using System.Collections.Generic;
using Planner.Api.DTOs.Requests.Goal;

namespace Planner.Api.Mappers.Goal;

public static class GoalMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<GoalResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<GoalDto>> dtos) =>
        new ResponseModel<IEnumerable<GoalResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<GoalResponseModel> ToResponseModel(this ServiceResult<GoalDto> dto) =>
        new ResponseModel<GoalResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<GoalResponseModel> ToResponseModel(this IEnumerable<GoalDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static GoalResponseModel ToResponseModel(this GoalDto dto) =>
        new GoalResponseModel
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateGoalDto ToDto(this CreateGoalRequestModel requestModel) =>
        new CreateGoalDto(
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}