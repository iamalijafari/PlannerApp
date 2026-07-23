using Planner.Api.DTOs.Responses.WeeklyPlan;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.WeeklyPlan;
using Planner.Api.DTOs.Requests.WeeklyPlan;

namespace Planner.Api.Mappers.WeeklyPlan;

public static class WeeklyPlanMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<WeeklyPlanResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<WeeklyPlanDto>> dtos) =>
        new ResponseModel<IEnumerable<WeeklyPlanResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<WeeklyPlanResponseModel> ToResponseModel(this ServiceResult<WeeklyPlanDto> dto) =>
        new ResponseModel<WeeklyPlanResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<WeeklyPlanResponseModel> ToResponseModel(this IEnumerable<WeeklyPlanDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static WeeklyPlanResponseModel ToResponseModel(this WeeklyPlanDto dto) =>
        new WeeklyPlanResponseModel
        {
            Id = dto.Id,
            MonthlyPlanId = dto.MonthlyPlanId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateWeeklyPlanDto ToDto(this CreateWeeklyPlanRequestModel requestModel) =>
        new CreateWeeklyPlanDto(
            requestModel.MonthlyPlanId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}