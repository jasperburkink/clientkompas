#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthenticationPoC/AuthenticationPoC.csproj", "AuthenticationPoC/"]
RUN dotnet restore "AuthenticationPoC/AuthenticationPoC.csproj"
COPY . .
WORKDIR "/src/AuthenticationPoC"
RUN dotnet build "AuthenticationPoC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationPoC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationPoC.dll"]