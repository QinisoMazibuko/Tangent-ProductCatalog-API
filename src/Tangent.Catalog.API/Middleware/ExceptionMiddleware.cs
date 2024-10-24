using System.Net;
using Tangent.Catalog.API.Constants;

namespace Tangent.Catalog.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Catch any custom exceptions
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Collections.Generic.KeyNotFoundException)
        {
            await context.Response.WriteCustomResponseAsync(HttpStatusCode.BadRequest, Defaults.NOT_FOUNT_ERROR_MESSAGE);
        }
        catch (Exception)
        {
            await context.Response.WriteCustomResponseAsync(HttpStatusCode.BadRequest, Defaults.INTERNAL_SERVER_ERROR_MESSAGE);
        }


    }

}
