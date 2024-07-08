#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0.6-bookworm-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0.302-1-bookworm-slim AS build
COPY . .
RUN ARCH=linux-x64 && \
    if [ "$(uname -m)" = "aarch64" ]; then ARCH=linux-arm64; fi && \
    echo "Building for ${ARCH}" &&\
    dotnet restore -r ${ARCH} && \
    dotnet build "./src/GenGithubAppInstallationToken/GenGithubAppInstallationToken.csproj" --no-restore -r ${ARCH} -c Release -o /app/build

FROM build AS publish
RUN ARCH=linux-x64 && \
    if [ "$(uname -m)" = "aarch64" ]; then ARCH=linux-arm64; fi && \
    echo "Publishing for ${ARCH}" &&\
    dotnet publish "./src/GenGithubAppInstallationToken/GenGithubAppInstallationToken.csproj" --no-restore -r ${ARCH} -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GenGithubAppInstallationToken.dll"]
