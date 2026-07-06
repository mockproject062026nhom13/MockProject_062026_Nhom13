# syntax=docker/dockerfile:1

# =============================================================================
# NursingHome.Api — multi-stage, non-root container image (.NET 10)
# Build context is expected to be the solution root (NursingHome/).
# =============================================================================

# ---------- Build stage -------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project/solution manifests first so `restore` is cached and only
# re-runs when a .csproj changes (not on every source edit).
COPY ["NursingHome.slnx", "./"]
COPY ["src/NursingHome.Api/NursingHome.Api.csproj", "src/NursingHome.Api/"]
COPY ["src/NursingHome.Application/NursingHome.Application.csproj", "src/NursingHome.Application/"]
COPY ["src/NursingHome.Domain/NursingHome.Domain.csproj", "src/NursingHome.Domain/"]
COPY ["src/NursingHome.Infrastructure/NursingHome.Infrastructure.csproj", "src/NursingHome.Infrastructure/"]

# Restoring the API restores its transitive project references as well.
RUN dotnet restore "src/NursingHome.Api/NursingHome.Api.csproj"

# Now bring in the rest of the source and publish a framework-dependent build.
COPY . .
RUN dotnet publish "src/NursingHome.Api/NursingHome.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# ---------- Runtime stage -----------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy the published output (owned by root, world-readable).
COPY --from=build /app/publish .

# Listen on an unprivileged port and stay in the container culture defaults.
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_EnableDiagnostics=0

EXPOSE 8080

# Drop privileges: $APP_UID is the non-root 'app' user shipped in the base image.
USER $APP_UID

ENTRYPOINT ["dotnet", "NursingHome.Api.dll"]
