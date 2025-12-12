using Planner.Application.Interfaces.Utilities;
using Planner.Application.Enumerations;

namespace Planner.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<GlobalExceptionMiddleware> logger;
    private readonly ITranslationUtility translationUtility;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        ITranslationUtility translationUtility)
    {
        this.next = next;
        this.logger = logger;
        this.translationUtility = translationUtility;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception caught by global middleware");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            string message = await translationUtility.Translate(MessageKey.ServerError, Language.fa);

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                error = message
            });
        }
    }
}