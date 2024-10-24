using System;
using System.Net;
using Tangent.Catalog.API.Constants;

namespace Tangent.Catalog.API.Middleware;

public class VersionMiddleware
{
    private readonly RequestDelegate _next;

    public VersionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        VerifyHeaderApiVersionAsync(context);

        if (!context.Response.HasStarted)
        {
            await _next(context);
        }
    }

    private static async void VerifyHeaderApiVersionAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(Defaults.HEADER_VERSION_NAME, out var headerValues))
        {
            if (headerValues != Defaults.API_VERSION)
            {
                await context.Response.WriteCustomResponseAsync(HttpStatusCode.BadRequest, Defaults.INCORRECT_VERSION_ERROR_MESSAGE);
            }
        }
    }
}
