using Planner.Api.DTOs.Responses.MonthlyGoal;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.MonthlyGoal;
using Planner.Api.DTOs.Requests.MonthlyGoal;

namespace Planner.Api.Mappers.MonthlyGoal;

public static class MonthlyGoalMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<MonthlyGoalResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<MonthlyGoalDto>> dtos) =>
        new ResponseModel<IEnumerable<MonthlyGoalResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<MonthlyGoalResponseModel> ToResponseModel(this ServiceResult<MonthlyGoalDto> dto) =>
        new ResponseModel<MonthlyGoalResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<MonthlyGoalResponseModel> ToResponseModel(this IEnumerable<MonthlyGoalDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static MonthlyGoalResponseModel ToResponseModel(this MonthlyGoalDto dto) =>
        new MonthlyGoalResponseModel
        {
            Id = dto.Id,
            YearlyGoalId = dto.YearlyGoalId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateMonthlyGoalDto ToDto(this CreateMonthlyGoalRequestModel requestModel) =>
        new CreateMonthlyGoalDto(
            requestModel.YearlyGoalId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}