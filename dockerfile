# Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy all solution structure
# NOTE: typically we need to copy only what's needed to economy space & memory
COPY . .

# Restore nuget packages
RUN dotnet restore

# Build whole solution including tests
RUN dotnet build PaymentsGateway.sln --no-restore

# Publish
FROM build as publish
WORKDIR /app
RUN dotnet publish ./src/Api.Host/Api.Host.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=publish /app/publish .
CMD [ "dotnet", "Api.Host.dll", "-d"]
