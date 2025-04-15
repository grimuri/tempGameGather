FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GameGather.Api/GameGather.Api.csproj", "GameGather.Api/"]
COPY ["GameGather.Application/GameGather.Application.csproj", "GameGather.Application/"]
COPY ["GameGather.Domain/GameGather.Domain.csproj", "GameGather.Domain/"]
COPY ["GameGather.Infrastructure/GameGather.Infrastructure.csproj", "GameGather.Infrastructure/"]
RUN dotnet restore "GameGather.Api/GameGather.Api.csproj"
COPY . .
WORKDIR "/src/GameGather.Api"
RUN dotnet build "GameGather.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GameGather.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameGather.Api.dll"]
