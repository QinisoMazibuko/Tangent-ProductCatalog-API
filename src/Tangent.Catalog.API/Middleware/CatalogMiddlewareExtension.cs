using System.Text.Json;
using System.Net;

namespace Tangent.Catalog.API.Middleware;

public static class CatalogMiddlewareExtension
{
    public static void UseCatalogMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseMiddleware<AuthorizationMiddleware>();
        app.UseMiddleware<VersionMiddleware>();
    }

    public static async Task WriteCustomResponseAsync(this HttpResponse response, HttpStatusCode statusCode, string message)
    {
        response.StatusCode = (int)statusCode;
        response.ContentType = "application/json";

        await response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = (int)statusCode,
            Message = message,
        }));
    }
}
