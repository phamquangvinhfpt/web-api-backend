#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5151
EXPOSE 5151

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Core/Core.csproj", "Core/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["DAO/DAO.csproj", "DAO/"]
COPY ["BussinessObject/BussinessObject.csproj", "BussinessObject/"]
RUN dotnet restore "./Core/Core.csproj"
COPY . .
WORKDIR "/src/Core"
RUN dotnet build "./Core.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:EnableSourceControlManagerQueries=false

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Core.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:EnableSourceControlManagerQueries=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.dll"]