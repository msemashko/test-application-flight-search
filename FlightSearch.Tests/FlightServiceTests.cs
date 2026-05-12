using FlightSearch.Application.DTOs;
using FlightSearch.Infrastructure.Services;

namespace FlightSearch.Tests;

public class FlightServiceTests
{
    private static FlightSearchRequest MakeRequest(
        string origin = "JFK",
        string destination = "LON",
        int passengers = 1,
        DateOnly? departureDate = null) =>
        new(origin, destination, departureDate, null, passengers, "oneway");

    [Fact]
    public async Task SearchAsync_ReturnsMatchingFlights()
    {
        using var db = TestDbFactory.Create();
        var service = new FlightService(db);

        var result = await service.SearchAsync(MakeRequest());

        Assert.Equal(2, result.Count);
        Assert.All(result, f =>
        {
            Assert.Equal("JFK", f.Origin);
            Assert.Equal("LON", f.Destination);
        });
    }

    [Fact]
    public async Task SearchAsync_FiltersOutFlightsWithNotEnoughSeats()
    {
        using var db = TestDbFactory.Create();
        var service = new FlightService(db);

        var result = await service.SearchAsync(MakeRequest(passengers: 5));

        Assert.Single(result);
        Assert.Equal("FS0001", result[0].FlightNumber);
    }

    [Fact]
    public async Task SearchAsync_ReturnsEmptyForNonExistentRoute()
    {
        using var db = TestDbFactory.Create();
        var service = new FlightService(db);

        var result = await service.SearchAsync(MakeRequest("PAR", "LON"));

        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_IsCaseInsensitive()
    {
        using var db = TestDbFactory.Create();
        var service = new FlightService(db);

        var result = await service.SearchAsync(MakeRequest("jfk", "lon"));

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchAsync_FiltersByDepartureDate()
    {
        using var db = TestDbFactory.Create();
        var service = new FlightService(db);

        var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var result = await service.SearchAsync(MakeRequest(departureDate: tomorrow));

        Assert.Equal(2, result.Count);
        Assert.All(result, f =>
            Assert.Equal(tomorrow, DateOnly.FromDateTime(f.DepartureTime)));
    }

    [Fact]
    public async Task SearchAsync_ReturnsEmptyForDateWithNoFlights()
    {
        using var db = TestDbFactory.Create();
        var service = new FlightService(db);

        var farFuture = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
        var result = await service.SearchAsync(MakeRequest(departureDate: farFuture));

        Assert.Empty(result);
    }
}
