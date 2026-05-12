using FlightSearch.Infrastructure.Services;

namespace FlightSearch.Tests;

public class AirportServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsAllAirportsSortedByCode()
    {
        using var db = TestDbFactory.Create();
        var service = new AirportService(db);

        var result = await service.GetAllAsync();

        Assert.Equal(3, result.Count);
        Assert.Equal("JFK", result[0].Code);
        Assert.Equal("LON", result[1].Code);
        Assert.Equal("PAR", result[2].Code);
    }

    [Fact]
    public async Task GetDestinationsAsync_ReturnsDestinationsForValidOrigin()
    {
        using var db = TestDbFactory.Create();
        var service = new AirportService(db);

        var result = await service.GetDestinationsAsync("JFK");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Code == "LON");
        Assert.Contains(result, d => d.Code == "PAR");
    }

    [Fact]
    public async Task GetDestinationsAsync_ReturnsNullForUnknownOrigin()
    {
        using var db = TestDbFactory.Create();
        var service = new AirportService(db);

        var result = await service.GetDestinationsAsync("XYZ");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDestinationsAsync_IsCaseInsensitive()
    {
        using var db = TestDbFactory.Create();
        var service = new AirportService(db);

        var result = await service.GetDestinationsAsync("jfk");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
