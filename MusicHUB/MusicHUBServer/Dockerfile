# Базовый образ с .NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Базовый образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файлы проектов и восстанавливаем зависимости
COPY ["MusicHUBServer/MusicHUBServer.csproj", "MusicHUBServer/"]
COPY ["HubClassLibrary/HubClassLibrary.csproj", "HubClassLibrary/"]
RUN dotnet restore "MusicHUBServer/MusicHUBServer.csproj"

# Копируем остальные файлы и собираем приложение
COPY . .
WORKDIR "/src/MusicHUBServer"
RUN dotnet build "MusicHUBServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "MusicHUBServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ для запуска
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicHUBServer.dll"]
