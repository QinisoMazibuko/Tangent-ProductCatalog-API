using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tangent.Catalog.API.Domain.Config;
using Tangent.Catalog.API.Domain.Contexts;
using Tangent.Catalog.API.Domain.Interfaces;
using Tangent.Catalog.API.Middleware;
using Tangent.Catalog.API.Repository.IoC;
using Tangent.Catalog.API.Services.IoC;

namespace Tangent.Catalog.API.Extensions;

public static class StartupExtensions
{
    public static void ConfigureTangentCatalogServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.ResponsePropertiesAndHeaders;
            options.RequestHeaders.Add("x-subscriber-id");
            options.RequestHeaders.Add("x-api-version");
            options.RequestHeaders.Add("x-appgw-trace-id");
        });

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddSwaggerGen(c =>
           {
               c.DescribeAllParametersInCamelCase();
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tangent.Catalog.API", Version = "v1" });
               c.AddSecurityDefinition("Bearer",
                   new OpenApiSecurityScheme
                   {
                       Description = "JWT Authorization header using the Bearer scheme.",
                       Type = SecuritySchemeType.Http,
                       Scheme = "bearer",
                   });

               c.AddSecurityDefinition("SubscriberId",
                   new OpenApiSecurityScheme
                   {
                       Description = "Subscriber guid header.",
                       Type = SecuritySchemeType.ApiKey,
                       Name = "x-subscriber-id",
                       In = ParameterLocation.Header,
                   });

               c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            },
                        },
                        new List<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "SubscriberId",
                                Type = ReferenceType.SecurityScheme,
                            },
                        },
                        new List<string>()
                    },
               });
           });

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        // Add JWT authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "tangent-identity-server",
                ValidAudience = "tangent-product-catalog-api",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("SubscriberPolicy", policy =>
            policy.RequireAuthenticatedUser());
        });

        // Register SubscriptionContext and IHttpContextAccessor
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ISubscriptionContext, SubscriptionContext>();

        //register service and repo layer services
        services.AddApplicationServices();
        services.AddRepositoryServices();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors();
    }

    public static void ConfigureTangentCatalogAPI(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseDeveloperExceptionPage();
        }


        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tangent Catalog API v1");
            c.RoutePrefix = string.Empty;
        });

        // register our custom middleware in the pipeline
        app.UseCatalogMiddleware(env);

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
