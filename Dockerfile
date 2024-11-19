#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["WSDemo.WebAPI/WSDemo.WebAPI.csproj", "WSDemo.WebAPI/"]
COPY ["WSDemo.Domain/WSDemo.Domain.csproj", "WSDemo.Domain/"]
COPY ["WSDemo.RepositoryLayer/WSDemo.RepositoryLayer.csproj", "WSDemo.RepositoryLayer/"]
COPY ["WSDemo.ServiceLayer/WSDemo.ServiceLayer.csproj", "WSDemo.ServiceLayer/"]
COPY ["WSDemo.SQLiteDB/WSDemo.SQLiteDB.csproj", "WSDemo.SQLiteDB/"]
RUN dotnet restore "./WSDemo.WebAPI/WSDemo.WebAPI.csproj"
COPY . .
WORKDIR "/src/WSDemo.WebAPI"
RUN dotnet build "./WSDemo.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./WSDemo.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WSDemo.WebAPI.dll"]

