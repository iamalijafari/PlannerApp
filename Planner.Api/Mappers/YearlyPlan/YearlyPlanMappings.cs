using Planner.Api.DTOs.Responses.YearlyPlan;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.YearlyPlan;
using System.Collections.Generic;
using System.Linq;
using Planner.Api.DTOs.Requests.YearlyPlan;

namespace Planner.Api.Mappers.YearlyPlan;

public static class YearlyPlanMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<YearlyPlanResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<YearlyPlanDto>> dtos) =>
        new ResponseModel<IEnumerable<YearlyPlanResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<YearlyPlanResponseModel> ToResponseModel(this ServiceResult<YearlyPlanDto> dto) =>
        new ResponseModel<YearlyPlanResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<YearlyPlanResponseModel> ToResponseModel(this IEnumerable<YearlyPlanDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static YearlyPlanResponseModel ToResponseModel(this YearlyPlanDto dto) =>
        new YearlyPlanResponseModel
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
    public static CreateYearlyPlanDto ToDto(this CreateYearlyPlanRequestModel requestModel) =>
        new CreateYearlyPlanDto(
            requestModel.GoalId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}