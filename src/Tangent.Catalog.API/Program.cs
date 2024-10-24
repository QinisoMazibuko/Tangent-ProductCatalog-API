using Tangent.Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureTangentCatalogServices(builder.Configuration);

var app = builder.Build();

// configure API middleware pipeline
app.ConfigureTangentCatalogAPI(app.Environment);

app.Run();
