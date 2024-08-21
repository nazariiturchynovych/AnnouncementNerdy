FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AnnouncementNerdy.API/AnnouncementNerdy.API.csproj", "AnnouncementNerdy.API/"]
COPY ["AnnouncementNerdy.Application/AnnouncementNerdy.Application.csproj", "AnnouncementNerdy.Application/"]
COPY ["AnnouncementNerdy.Domain/AnnouncementNerdy.Domain.csproj", "AnnouncementNerdy.Domain/"]
COPY ["AnnouncementNerdy.Infrastructure/AnnouncementNerdy.Infrastructure.csproj", "AnnouncementNerdy.Infrastructure/"]
RUN dotnet restore "AnnouncementNerdy.API/AnnouncementNerdy.API.csproj"
COPY . .
WORKDIR "/src/AnnouncementNerdy.API"
RUN dotnet build "AnnouncementNerdy.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AnnouncementNerdy.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnnouncementNerdy.API.dll"]
