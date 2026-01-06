# 1️⃣ Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
USER app

# 2️⃣ Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files (csproj)
COPY src/Inventories/Inventories.Api/Inventories.Api.csproj Inventories.Api/
COPY src/Inventories/Inventories.Application/Inventories.Application.csproj Inventories.Application/
COPY src/Inventories/Inventories.Contracts/Inventories.Contracts.csproj Inventories.Contracts/
COPY src/Inventories/Inventories.Domain/Inventories.Domain.csproj Inventories.Domain/
COPY src/Inventories/Inventories.Infrastructure/Inventories.Infrastructure.csproj Inventories.Infrastructure/
COPY Nuget.config ./

# Restore dependencies
RUN dotnet restore Inventories.Api/Inventories.Api.csproj

# Copy all source code
COPY src/Inventories/Inventories.Api/. Inventories.Api/
COPY src/Inventories/Inventories.Application/. Inventories.Application/
COPY src/Inventories/Inventories.Contracts/. Inventories.Contracts/
COPY src/Inventories/Inventories.Domain/. Inventories.Domain/
COPY src/Inventories/Inventories.Infrastructure/. Inventories.Infrastructure/

# 3️⃣ Publish
FROM build AS publish
RUN dotnet publish Inventories.Api/Inventories.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# 4️⃣ Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventories.Api.dll"]
