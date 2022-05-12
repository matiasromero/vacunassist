FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

VOLUME ~/.aspnet/https /https:ro

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["VacunnasistBackend.csproj", "./"]
RUN dotnet restore "VacunnasistBackend.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "VacunnasistBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacunnasistBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacunnasistBackend.dll"]
