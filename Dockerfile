FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ---------------- BUILD ----------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia tudo (solution inteira)
COPY . .

# Vai até o projeto da API
WORKDIR /src/src/API

RUN dotnet restore
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# ---------------- PUBLISH ----------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------------- FINAL ----------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Produtos-rabbitMq.dll"]
