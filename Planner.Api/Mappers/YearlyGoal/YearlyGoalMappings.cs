using Planner.Api.DTOs.Responses.YearlyGoal;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.YearlyGoal;
using System.Collections.Generic;
using System.Linq;
using Planner.Api.DTOs.Requests.YearlyGoal;

namespace Planner.Api.Mappers.YearlyGoal;

public static class YearlyGoalMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<YearlyGoalResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<YearlyGoalDto>> dtos) =>
        new ResponseModel<IEnumerable<YearlyGoalResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<YearlyGoalResponseModel> ToResponseModel(this ServiceResult<YearlyGoalDto> dto) =>
        new ResponseModel<YearlyGoalResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<YearlyGoalResponseModel> ToResponseModel(this IEnumerable<YearlyGoalDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static YearlyGoalResponseModel ToResponseModel(this YearlyGoalDto dto) =>
        new YearlyGoalResponseModel
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
    public static CreateYearlyGoalDto ToDto(this CreateYearlyGoalRequestModel requestModel) =>
        new CreateYearlyGoalDto(
            requestModel.GoalId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}