# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV TZ=America/Sao_Paulo
RUN cat /usr/share/zoneinfo/$TZ > /etc/localtime \
		&& cat /usr/share/zoneinfo/$TZ > /etc/timezone \
		&& update-ca-certificates \
        rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY  ./websocket/src/Jornada.Websocket/ ./Jornada.Websocket/
COPY ./aspire/src/Jornada.ServiceDefaults/ ./../../../aspire/src/Jornada.ServiceDefaults/
WORKDIR /src/Jornada.Websocket
RUN dotnet restore ./Jornada.Websocket.csproj
RUN dotnet restore ./../../../aspire/src/Jornada.ServiceDefaults/Jornada.ServiceDefaults.csproj

COPY ./websocket/src/Jornada.Websocket/ .
RUN dotnet build ./Jornada.Websocket.csproj -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ./Jornada.Websocket.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jornada.Websocket.dll"]