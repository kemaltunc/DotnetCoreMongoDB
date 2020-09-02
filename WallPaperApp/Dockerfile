FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["WallPaperApp/WallPaperApp.csproj", "WallPaperApp/"]
RUN dotnet restore "WallPaperApp/WallPaperApp.csproj"
COPY . .
WORKDIR "/src/WallPaperApp"
RUN dotnet build "WallPaperApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WallPaperApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WallPaperApp.dll"]
