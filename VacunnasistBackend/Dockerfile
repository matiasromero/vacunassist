FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

ENV TZ=America/Argentina/Buenos_Aires
RUN echo "America/Argentina/Buenos_Aires" > /etc/timezone
RUN ln -fs /usr/share/zoneinfo/$TZ /etc/localtime
RUN dpkg-reconfigure -f noninteractive tzdata

WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

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

