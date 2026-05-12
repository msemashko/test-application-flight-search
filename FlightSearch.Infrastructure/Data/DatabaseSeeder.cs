using FlightSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(FlightSearchDbContext db)
    {
        var airportsExist = await db.Airports.AnyAsync();

        if (!airportsExist)
        {
            var airports = new List<Airport>
            {
                new() { Code = "JFK", Name = "John F. Kennedy International", City = "New York", Country = "US" },
                new() { Code = "LAX", Name = "Los Angeles International", City = "Los Angeles", Country = "US" },
                new() { Code = "LON", Name = "Heathrow", City = "London", Country = "UK" },
                new() { Code = "PAR", Name = "Charles de Gaulle", City = "Paris", Country = "FR" },
                new() { Code = "BLR", Name = "Kempegowda International", City = "Bangalore", Country = "IN" },
                new() { Code = "DXB", Name = "Dubai International", City = "Dubai", Country = "AE" },
                new() { Code = "SIN", Name = "Changi", City = "Singapore", Country = "SG" },
                new() { Code = "NRT", Name = "Narita International", City = "Tokyo", Country = "JP" },
            };

            db.Airports.AddRange(airports);

            var routeMap = new Dictionary<string, string[]>
            {
                ["JFK"] = ["LON", "PAR", "LAX", "DXB"],
                ["LAX"] = ["JFK", "NRT", "SIN"],
                ["LON"] = ["JFK", "PAR", "DXB", "BLR"],
                ["PAR"] = ["LON", "JFK", "DXB"],
                ["BLR"] = ["LON", "DXB", "SIN"],
                ["DXB"] = ["JFK", "LON", "PAR", "BLR", "SIN"],
                ["SIN"] = ["LAX", "NRT", "DXB", "BLR"],
                ["NRT"] = ["LAX", "SIN"],
            };

            var routes = routeMap
                .SelectMany(kvp => kvp.Value.Select(dest => new Route
                {
                    OriginCode = kvp.Key,
                    DestinationCode = dest
                }))
                .ToList();

            db.Routes.AddRange(routes);
            await db.SaveChangesAsync();
        }

        await ReseedFlightsAsync(db);
    }

    private static async Task ReseedFlightsAsync(FlightSearchDbContext db)
    {
        var hasCurrentFlights = await db.Flights.AnyAsync(f => f.DepartureTime >= DateTime.Today);
        if (hasCurrentFlights)
            return;

        db.Flights.RemoveRange(db.Flights);

        var routes = await db.Routes.ToListAsync();
        var random = new Random(42);
        var seq = 1;

        foreach (var route in routes)
        {
            for (var day = 1; day <= 7; day++)
            {
                var baseDate = DateTime.Today.AddDays(day);
                for (var i = 0; i < 2; i++)
                {
                    var departure = baseDate.AddHours(6 + i * 8 + random.Next(0, 3));
                    var duration = TimeSpan.FromHours(2 + random.Next(1, 10));
                    db.Flights.Add(new Flight
                    {
                        FlightNumber = $"FS{seq++:D4}",
                        OriginCode = route.OriginCode,
                        DestinationCode = route.DestinationCode,
                        DepartureTime = departure,
                        ArrivalTime = departure + duration,
                        Price = Math.Round((decimal)(150 + random.Next(0, 800) + random.NextDouble()), 2),
                        AvailableSeats = random.Next(1, 180)
                    });
                }
            }
        }

        await db.SaveChangesAsync();
    }
}
