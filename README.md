# Flight Search

## 1. Visual Studio

1. Open `FlightSearch.sln` in Visual Studio 2022/2026.
2. Set **FlightSearch.Server** as the startup project.
3. Press **F5** (or Ctrl+F5 to run without debugger).

Visual Studio will restore NuGet/npm packages, start the backend, and launch the Vite dev server automatically via SPA proxy.

| What | URL |
|------|-----|
| Frontend | https://localhost:61998 |
| Swagger UI | http://localhost:5003/swagger |
| OpenAPI spec | http://localhost:5003/openapi/v1.json |

## 2. Terminal

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)

### Steps

```bash
cd flightsearch.client
npm install
cd ../FlightSearch.Server
dotnet run
```

The SPA proxy automatically starts the Vite dev server alongside the backend.

| What | URL |
|------|-----|
| Frontend | https://localhost:61998 |
| Swagger UI | http://localhost:5003/swagger |
| OpenAPI spec | http://localhost:5003/openapi/v1.json |

## 3. Docker / Podman

### Prerequisites

- [Docker](https://www.docker.com/) or [Podman](https://podman.io/)

### Steps

```bash
# Docker
docker compose up --build

# Podman (recommended on Windows without Docker Desktop)
podman compose up --build
```

The container builds both the .NET backend and the Vue frontend and serves everything from a single port.

| What | URL |
|------|-----|
| Application | http://localhost:5003 |
| Swagger UI | http://localhost:5003/swagger |
| OpenAPI spec | http://localhost:5003/openapi/v1.json |

## Example API Requests

```bash
# List all airports
curl http://localhost:5003/api/airports

# Get destinations from JFK
curl http://localhost:5003/api/airports/destinations/JFK

# Search one-way flights
curl "http://localhost:5003/api/flights/search?origin=JFK&destination=LON&passengers=1&departureDate=2026-05-15&tripType=oneway"

# Search return flights
curl "http://localhost:5003/api/flights/search?origin=JFK&destination=LON&passengers=2&departureDate=2026-05-15&returnDate=2026-05-20&tripType=return"

# Invalid origin (400 Bad Request)
curl http://localhost:5003/api/airports/destinations/X

# Unknown origin (404 Not Found)
curl http://localhost:5003/api/airports/destinations/ZZZ
```

## Test Data

The database is seeded on startup with flights for the **next 7 days** (relative to today). Each route gets 2 flights per day. If you see "No flights found", make sure the departure date is between **tomorrow** and **7 days from now**.

Available airports: `JFK`, `LAX`, `LON`, `PAR`, `BLR`, `DXB`, `SIN`, `NRT`.
