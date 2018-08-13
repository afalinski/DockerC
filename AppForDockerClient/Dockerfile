FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /AppForDockerClient
COPY *.csproj AppForDockerClient/
RUN dotnet restore AppForDockerClient/AppForDockerClient.csproj
COPY . AppForDockerClient/
WORKDIR AppForDockerClient
RUN dotnet build AppForDockerClient.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AppForDockerClient.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AppForDockerClient.dll"]
