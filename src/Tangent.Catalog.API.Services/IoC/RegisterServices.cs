using System;
using Microsoft.Extensions.DependencyInjection;
using Tangent.Catalog.API.Services.Interfaces;
using Tangent.Catalog.API.Services.Mappers;
using Tangent.Catalog.API.Services.Services;

namespace Tangent.Catalog.API.Services.IoC;

public static class RegisterServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IProductService, ProductService>();
        services.AddAutoMapper(typeof(DomainMappingProfile));
    }

}
