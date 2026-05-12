using FlightSearch.Application.Interfaces;
using FlightSearch.Application.DTOs;
using FlightSearch.Application.Validators;
using FlightSearch.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FlightSearch.Tests;

public class AirportsControllerTests
{
    private readonly IAirportService _airportService = Substitute.For<IAirportService>();
    private readonly AirportsController _controller;

    public AirportsControllerTests()
    {
        var validator = new DestinationsRequestValidator();
        _controller = new AirportsController(_airportService, validator);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithAirports()
    {
        var airports = new List<AirportDto> { new("JFK", "JFK International", "New York", "US") };
        _airportService.GetAllAsync().Returns(airports);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(airports, ok.Value);
    }

    [Fact]
    public async Task GetDestinations_ReturnsBadRequestForInvalidCode()
    {
        var result = await _controller.GetDestinations("X");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetDestinations_ReturnsNotFoundForUnknownOrigin()
    {
        _airportService.GetDestinationsAsync("XYZ").Returns((List<AirportDto>?)null);

        var result = await _controller.GetDestinations("XYZ");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetDestinations_ReturnsOkWithDestinations()
    {
        var destinations = new List<AirportDto> { new("LON", "Heathrow", "London", "UK") };
        _airportService.GetDestinationsAsync("JFK").Returns(destinations);

        var result = await _controller.GetDestinations("JFK");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(destinations, ok.Value);
    }
}
