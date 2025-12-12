using Planner.Application.Enumerations;

namespace Planner.Api.DTOs.Responses.Utility;

public class ResponseModel<T>
{
    public bool Success { get; set; }
    public T Result { get; set; }
    public MessageKey MessageKey { get; set; }
}