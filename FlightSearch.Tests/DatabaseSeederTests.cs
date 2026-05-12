using FlightSearch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.Tests;

public class DatabaseSeederTests
{
    [Fact]
    public async Task SeedAsync_PopulatesEmptyDatabase()
    {
        using var db = TestDbFactory.Create();
        var emptyDb = new FlightSearchDbContext(
            new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<FlightSearchDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
        emptyDb.Database.EnsureCreated();

        await DatabaseSeeder.SeedAsync(emptyDb);

        Assert.True(emptyDb.Airports.Any());
        Assert.True(emptyDb.Routes.Any());
        Assert.True(emptyDb.Flights.Any());
    }

    [Fact]
    public async Task SeedAsync_DoesNotDuplicateDataOnSecondRun()
    {
        using var db = TestDbFactory.Create();
        var freshDb = new FlightSearchDbContext(
            new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<FlightSearchDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
        freshDb.Database.EnsureCreated();

        await DatabaseSeeder.SeedAsync(freshDb);
        var countAfterFirst = freshDb.Airports.Count();

        await DatabaseSeeder.SeedAsync(freshDb);
        var countAfterSecond = freshDb.Airports.Count();

        Assert.Equal(countAfterFirst, countAfterSecond);
    }
}
