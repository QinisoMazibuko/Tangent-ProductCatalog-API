using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tangent.Catalog.API.Repository.Cache;
using Tangent.Catalog.API.Repository.Interfaces;
using Tangent.Catalog.API.Repository.Mappers;
using Tangent.Catalog.API.Repository.Repositories;

namespace Tangent.Catalog.API.Repository.IoC;

public static class RegisterServices
{
    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();

        services.AddLazyCache();
        services.AddSingleton<IProductCache, ProductCache>();

        services.AddAutoMapper(typeof(ProductMappingProfile));
    }

}
