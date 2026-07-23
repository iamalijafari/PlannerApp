using Planner.Api.DTOs.Responses.DailyGoal;
using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;
using Planner.Application.DTOs.DailyGoal;
using Planner.Api.DTOs.Requests.DailyGoal;

namespace Planner.Api.Mappers.DailyGoal;

public static class DailyGoalMappings
{
    #region ToResponseModel
    public static ResponseModel<IEnumerable<DailyGoalResponseModel>> ToResponseModel(this ServiceResult<IEnumerable<DailyGoalDto>> dtos) =>
        new ResponseModel<IEnumerable<DailyGoalResponseModel>>
        {
            Success = dtos.Success,
            Result = dtos.Result?.ToResponseModel(),
            MessageKey = dtos.MessageKey
        };

    public static ResponseModel<DailyGoalResponseModel> ToResponseModel(this ServiceResult<DailyGoalDto> dto) =>
        new ResponseModel<DailyGoalResponseModel>
        {
            Success = dto.Success,
            Result = dto.Result?.ToResponseModel(),
            MessageKey = dto.MessageKey
        };

    public static IEnumerable<DailyGoalResponseModel> ToResponseModel(this IEnumerable<DailyGoalDto> dtos) =>
        dtos.Select(dto => dto.ToResponseModel());

    public static DailyGoalResponseModel ToResponseModel(this DailyGoalDto dto) =>
        new DailyGoalResponseModel
        {
            Id = dto.Id,
            WeeklyGoalId = dto.WeeklyGoalId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedAt = dto.CreatedAt,
            DueDate = dto.DueDate,
            IsCompleted = dto.IsCompleted
        };
    #endregion
    #region ToDto
    public static CreateDailyGoalDto ToDto(this CreateDailyGoalRequestModel requestModel) =>
        new CreateDailyGoalDto(
            requestModel.WeeklyGoalId,
            requestModel.Title,
            requestModel.Description,
            requestModel.DueDate);
    #endregion
}