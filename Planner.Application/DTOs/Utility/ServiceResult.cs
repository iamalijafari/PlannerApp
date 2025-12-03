using Planner.Application.Enumerations;
using Planner.Application.Utilities;

namespace Planner.Application.DTOs.Utility;

public class ServiceResult<T>
{
    public bool Success { get; private set; }
    public T Result { get; private set; }
    public MessageKey MessageKey { get; private set; }

    public void SetResult(T result)
    {
        Result = result;
        Success = true;
    }

    public void SetError(MessageKey message)
    {
        MessageKey = message;
        Success = false;
    }
}