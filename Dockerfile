# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/SmartLearn.API/SmartLearn.API.csproj", "src/SmartLearn.API/"]
COPY ["src/SmartLearn.Application/SmartLearn.Application.csproj", "src/SmartLearn.Application/"]
COPY ["src/SmartLearn.Domain/SmartLearn.Domain.csproj", "src/SmartLearn.Domain/"]
COPY ["src/SmartLearn.Infrastructure/SmartLearn.Infrastructure.csproj", "src/SmartLearn.Infrastructure/"]

RUN dotnet restore "src/SmartLearn.API/SmartLearn.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/SmartLearn.API"
RUN dotnet build "SmartLearn.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SmartLearn.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install necessary packages for AI services
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "SmartLearn.API.dll"]