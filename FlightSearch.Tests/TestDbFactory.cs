using FlightSearch.Domain.Entities;
using FlightSearch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.Tests;

public static class TestDbFactory
{
    public static FlightSearchDbContext Create()
    {
        var options = new DbContextOptionsBuilder<FlightSearchDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new FlightSearchDbContext(options);
        db.Database.EnsureCreated();

        db.Airports.AddRange(
            new Airport { Code = "JFK", Name = "JFK International", City = "New York", Country = "US" },
            new Airport { Code = "LON", Name = "Heathrow", City = "London", Country = "UK" },
            new Airport { Code = "PAR", Name = "Charles de Gaulle", City = "Paris", Country = "FR" }
        );

        db.Routes.AddRange(
            new Route { OriginCode = "JFK", DestinationCode = "LON" },
            new Route { OriginCode = "JFK", DestinationCode = "PAR" },
            new Route { OriginCode = "LON", DestinationCode = "JFK" }
        );

        db.Flights.AddRange(
            new Flight
            {
                FlightNumber = "FS0001",
                OriginCode = "JFK",
                DestinationCode = "LON",
                DepartureTime = DateTime.Today.AddDays(1).AddHours(8),
                ArrivalTime = DateTime.Today.AddDays(1).AddHours(16),
                Price = 450.00m,
                AvailableSeats = 120
            },
            new Flight
            {
                FlightNumber = "FS0002",
                OriginCode = "JFK",
                DestinationCode = "LON",
                DepartureTime = DateTime.Today.AddDays(1).AddHours(14),
                ArrivalTime = DateTime.Today.AddDays(1).AddHours(22),
                Price = 380.00m,
                AvailableSeats = 3
            },
            new Flight
            {
                FlightNumber = "FS0003",
                OriginCode = "LON",
                DestinationCode = "JFK",
                DepartureTime = DateTime.Today.AddDays(2).AddHours(10),
                ArrivalTime = DateTime.Today.AddDays(2).AddHours(18),
                Price = 520.00m,
                AvailableSeats = 80
            }
        );

        db.SaveChanges();
        return db;
    }
}
