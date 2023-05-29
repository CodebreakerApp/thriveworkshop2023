#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["api/GamesAPI/GamesAPI.csproj", "api/GamesAPI/"]
COPY ["common/CodeBreaker.Shared/CodeBreaker.Shared.csproj", "common/CodeBreaker.Shared/"]
COPY ["data/Codebreaker.Data/Codebreaker.Data.csproj", "data/Codebreaker.Data/"]
RUN dotnet restore "api/GamesAPI/GamesAPI.csproj"
COPY . .
WORKDIR "/src/api/GamesAPI"
RUN dotnet build "GamesAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GamesAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GamesAPI.dll"]