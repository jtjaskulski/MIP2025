# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["BombPol.WWW/BombPol.WWW.csproj", "./BombPol.WWW/"]
COPY ["BombPol.Data/BombPol.Data.csproj", "./BombPol.Data/"]
RUN dotnet restore "./BombPol.WWW/BombPol.WWW.csproj"

# Copy everything else and build
COPY BombPol.WWW/. ./BombPol.WWW/
COPY BombPol.Data/. ./BombPol.Data/
WORKDIR "/src/BombPol.WWW"
RUN dotnet build "./BombPol.WWW.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BombPol.WWW.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BombPol.WWW.dll"]
