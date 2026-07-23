using Planner.Api.DTOs.Responses.MonthlyPlan;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.MonthlyPlan;
using Planner.Api.DTOs.Requests.MonthlyPlan;

namespace Planner.Api.Mappers.MonthlyPlan;

public static class MonthlyPlanMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<MonthlyPlanResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<MonthlyPlanDto>> dtos) =>
        new ResponseModel<IEnumerable<MonthlyPlanResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<MonthlyPlanResponseModel> ToResponseModel(this ServiceResult<MonthlyPlanDto> dto) =>
        new ResponseModel<MonthlyPlanResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<MonthlyPlanResponseModel> ToResponseModel(this IEnumerable<MonthlyPlanDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static MonthlyPlanResponseModel ToResponseModel(this MonthlyPlanDto dto) =>
        new MonthlyPlanResponseModel
        {
            Id = dto.Id,
            YearlyPlanId = dto.YearlyPlanId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateMonthlyPlanDto ToDto(this CreateMonthlyPlanRequestModel requestModel) =>
        new CreateMonthlyPlanDto(
            requestModel.YearlyPlanId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}