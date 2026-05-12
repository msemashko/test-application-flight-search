FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS with-node
RUN apt-get update && apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get install -y nodejs


FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FlightSearch.Server/FlightSearch.Server.csproj", "FlightSearch.Server/"]
COPY ["FlightSearch.Application/FlightSearch.Application.csproj", "FlightSearch.Application/"]
COPY ["FlightSearch.Domain/FlightSearch.Domain.csproj", "FlightSearch.Domain/"]
COPY ["FlightSearch.Infrastructure/FlightSearch.Infrastructure.csproj", "FlightSearch.Infrastructure/"]
COPY ["flightsearch.client/flightsearch.client.esproj", "flightsearch.client/"]
COPY ["flightsearch.client/package.json", "flightsearch.client/"]
COPY ["flightsearch.client/package-lock.json", "flightsearch.client/"]
RUN dotnet restore "./FlightSearch.Server/FlightSearch.Server.csproj"
COPY . .
WORKDIR "/src/FlightSearch.Server"
RUN dotnet build "./FlightSearch.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FlightSearch.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightSearch.Server.dll"]
