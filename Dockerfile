# Use the official .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TelemetryGen.csproj", "./"]
RUN dotnet restore "TelemetryGen.csproj"
COPY . .
RUN dotnet build "TelemetryGen.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TelemetryGen.csproj" -c Release -o /app/publish

# Final stage - runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelemetryGen.dll"]