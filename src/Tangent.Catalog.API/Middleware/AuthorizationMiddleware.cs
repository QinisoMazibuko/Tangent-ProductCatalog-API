using System.Net;
using Microsoft.AspNetCore.Authorization;
using Tangent.Catalog.API.Constants;
using Tangent.Catalog.API.Domain.Interfaces;

namespace Tangent.Catalog.API.Middleware;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISubscriptionContext subscriptionContext)
    {
        if (context != null)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() == null)
            {
                // No authorization required for this endpoint, skip the middleware
                await _next(context);
                return;
            }

            var currentSubcriber = subscriptionContext.GetCurrentSubscriber();
            if (currentSubcriber != null)
            {
                if (await IsValidSubscriber(currentSubcriber.SubscriberId))
                {
                    await _next(context);
                    return;
                }
            }

            await context.Response.WriteCustomResponseAsync(HttpStatusCode.Forbidden, Defaults.NOT_AUTHORIZED_ERROR_MESSAGE);
        }
    }

    /// <summary>
    /// simulate checking a user store to validate a subscriber ID from the token is valid
    /// </summary>
    /// <param name="subscriberId"></param>
    /// <returns></returns>
    public async Task<bool> IsValidSubscriber(string subscriberId)
    {
        var user = await DefaultUserContext.GetUser(subscriberId);

        return user != null && !string.IsNullOrEmpty(user.Name);
    }

}
