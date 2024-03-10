#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG ENVIRONMENT_NAME=Development
RUN echo Using Environment for build stage: ${ENVIRONMENT_NAME}

WORKDIR /src
COPY ["API/API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build /p:EnvironmentName=${ENVIRONMENT_NAME} "API.csproj" -c Release -o /app/build

FROM build AS publish
ARG ENVIRONMENT_NAME=Development
RUN echo Using Environment for publish stage: ${ENVIRONMENT_NAME}

RUN dotnet publish /p:EnvironmentName=${ENVIRONMENT_NAME} "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG ENVIRONMENT_NAME=Development
RUN echo Using Environment for final stage: ${ENVIRONMENT_NAME}
ENV DOTNET_ENVIRONMENT_NAME=${ENVIRONMENT_NAME}
RUN echo Setting DOTNET_ENVIRONMENT_NAME variable to: $DOTNET_ENVIRONMENT_NAME

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet API.dll environment=$DOTNET_ENVIRONMENT_NAME