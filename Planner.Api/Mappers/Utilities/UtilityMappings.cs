using Microsoft.AspNetCore.Http;
using Planner.Application.DTOs.Utility;
using Planner.Application.Interfaces.Utilities;
using Planner.Application.Enumerations;

namespace Planner.Api.Mappers.Utilities;

public static class UtilityMappings
{
    public static IServiceProvider ServiceProvider { get; set; }
    
    private static ITranslationUtility GetTranslationUtility() => ServiceProvider.GetRequiredService<ITranslationUtility>();

    public static IResult ToResult<T>(this ServiceResult<T> serviceResult)
    {
        if (serviceResult.Success)
        {
            return Results.Json(serviceResult.Result, statusCode: 200);
        }

        return Results.Json(new { error = GetTranslationUtility().Translate(serviceResult.MessageKey, Language.fa) }, statusCode: 500);
    }
}