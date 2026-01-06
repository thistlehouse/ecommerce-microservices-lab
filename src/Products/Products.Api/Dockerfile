# 1️⃣ Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
USER app

# 2️⃣ Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files (csproj)
COPY src/Products/Products.Api/Products.Api.csproj Products.Api/
COPY src/Products/Products.Application/Products.Application.csproj Products.Application/
COPY src/Products/Products.Contracts/Products.Contracts.csproj Products.Contracts/
COPY src/Products/Products.Domain/Products.Domain.csproj Products.Domain/
COPY src/Products/Products.Infrastructure/Products.Infrastructure.csproj Products.Infrastructure/
COPY Nuget.config ./

# Restore dependencies
RUN dotnet restore Products.Api/Products.Api.csproj

# Copy all source code
COPY src/Products/Products.Api/. Products.Api/
COPY src/Products/Products.Application/. Products.Application/
COPY src/Products/Products.Contracts/. Products.Contracts/
COPY src/Products/Products.Domain/. Products.Domain/
COPY src/Products/Products.Infrastructure/. Products.Infrastructure/

# 3️⃣ Publish
FROM build AS publish
RUN dotnet publish Products.Api/Products.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# 4️⃣ Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.Api.dll"]
