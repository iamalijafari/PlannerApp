using Planner.Api.DTOs.Responses.Utility;
using Planner.Application.DTOs.Utility;

namespace Planner.Api.Mappers.General;

public static class GeneralMappings
{
    #region ToResponseModel
    public static ResponseModel<bool> ToResponseModel(this ServiceResult<bool> dto) =>
        new ResponseModel<bool>
        {
            Success = dto.Success,
            Result = dto.Result,
            MessageKey = dto.MessageKey
        };
    #endregion
}