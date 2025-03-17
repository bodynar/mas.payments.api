# 1. Используем официальный образ ASP.NET 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# 2. Билд образа на основе SDK 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 3. Копируем файлы проекта
COPY ["MAS.Payments.csproj", "./"]
RUN dotnet restore "./MAS.Payments.csproj"

# 4. Копируем все файлы и собираем приложение
COPY . .
RUN dotnet publish -c Release -o /app/publish

# 5. Переходим в финальный runtime-образ
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# 6. Указываем точку входа
ENTRYPOINT ["dotnet", "MAS.Payments.dll"]
