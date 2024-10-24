# Use the .NET 8.0 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src
EXPOSE 8080

# Copy csproj and restore as distinct layers
COPY ["src/Tangent.Catalog.API/Tangent.Catalog.API.csproj", "src/Tangent.Catalog.API/"]
RUN dotnet restore "src/Tangent.Catalog.API/Tangent.Catalog.API.csproj"

# Copy the rest of the files and build the app
COPY . .
WORKDIR "src/Tangent.Catalog.API"
RUN dotnet build "Tangent.Catalog.API.csproj" -c Release -o /app/build


# Publish the app
FROM build-env AS publish
RUN dotnet publish "Tangent.Catalog.API.csproj" -c Release -o /app/publish

# Use the .NET 8.0 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Tangent.Catalog.API.dll"]
