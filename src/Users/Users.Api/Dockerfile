# 1️⃣ Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
USER app

# 2️⃣ Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files (csproj)
COPY src/Users/Users.Api/Users.Api.csproj Users.Api/
COPY src/Users/Users.Application/Users.Application.csproj Users.Application/
COPY src/Users/Users.Contracts/Users.Contracts.csproj Users.Contracts/
COPY src/Users/Users.Domain/Users.Domain.csproj Users.Domain/
COPY src/Users/Users.Infrastructure/Users.Infrastructure.csproj Users.Infrastructure/
COPY Nuget.config ./

# Restore dependencies
RUN dotnet restore Users.Api/Users.Api.csproj

# Copy all source code
COPY src/Users/Users.Api/. Users.Api/
COPY src/Users/Users.Application/. Users.Application/
COPY src/Users/Users.Contracts/. Users.Contracts/
COPY src/Users/Users.Domain/. Users.Domain/
COPY src/Users/Users.Infrastructure/. Users.Infrastructure/

# 3️⃣ Publish
FROM build AS publish
RUN dotnet publish Users.Api/Users.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# 4️⃣ Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Api.dll"]
