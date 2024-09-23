FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AutoBookKeeper.Web/AutoBookKeeper.Web.csproj", "src/AutoBookKeeper.Web/"]
COPY ["src/AutoBookKeeper.Application/AutoBookKeeper.Application.csproj", "src/AutoBookKeeper.Application/"]
COPY ["src/AutoBookKeeper.Core/AutoBookKeeper.Core.csproj", "src/AutoBookKeeper.Core/"]
COPY ["src/AutoBookKeeper.Infrastructure/AutoBookKeeper.Infrastructure.csproj", "src/AutoBookKeeper.Infrastructure/"]
RUN dotnet restore "src/AutoBookKeeper.Web/AutoBookKeeper.Web.csproj"
COPY . .
WORKDIR "/src/src/AutoBookKeeper.Web"
RUN dotnet build "AutoBookKeeper.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AutoBookKeeper.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoBookKeeper.Web.dll"]
