#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8089
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["zackStudy/IdService.WebApi.csproj", "zackStudy/"]
COPY ["IdService.DomainService/IdService.DomainService.csproj", "IdService.DomainService/"]
COPY ["raok.Common/Raok.Common.csproj", "raok.Common/"]
COPY ["Raok.JWT/Raok.JWT.csproj", "Raok.JWT/"]
COPY ["IdService.Infrastructure/IdService.Infrastructure.csproj", "IdService.Infrastructure/"]
COPY ["Raok.CommonInitializer/Raok.CommonInitializer.csproj", "Raok.CommonInitializer/"]
COPY ["Raok.Infrastructure/Raok.Infrastructure.csproj", "Raok.Infrastructure/"]
RUN dotnet restore "zackStudy/IdService.WebApi.csproj"
COPY . .
WORKDIR "/src/zackStudy"
RUN dotnet build "IdService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdService.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdService.WebApi.dll"]