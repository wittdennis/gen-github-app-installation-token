#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0.6-alpine3.19 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY . .
RUN dotnet restore
RUN dotnet build "./src/GenGithubAppInstallationToken/GenGithubAppInstallationToken.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/GenGithubAppInstallationToken/GenGithubAppInstallationToken.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GenGithubAppInstallationToken.dll"]
