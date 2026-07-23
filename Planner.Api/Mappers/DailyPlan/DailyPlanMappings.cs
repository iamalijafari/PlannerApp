using Planner.Api.DTOs.Responses.DailyPlan;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.DailyPlan;
using Planner.Api.DTOs.Requests.DailyPlan;

namespace Planner.Api.Mappers.DailyPlan;

public static class DailyPlanMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<DailyPlanResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<DailyPlanDto>> dtos) =>
        new ResponseModel<IEnumerable<DailyPlanResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<DailyPlanResponseModel> ToResponseModel(this ServiceResult<DailyPlanDto> dto) =>
        new ResponseModel<DailyPlanResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<DailyPlanResponseModel> ToResponseModel(this IEnumerable<DailyPlanDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static DailyPlanResponseModel ToResponseModel(this DailyPlanDto dto) =>
        new DailyPlanResponseModel
        {
            Id = dto.Id,
            WeeklyPlanId = dto.WeeklyPlanId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateDailyPlanDto ToDto(this CreateDailyPlanRequestModel requestModel) =>
        new CreateDailyPlanDto(
            requestModel.WeeklyPlanId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}