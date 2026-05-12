using FlightSearch.Application.Interfaces;
using FlightSearch.Application.DTOs;
using FlightSearch.Application.Validators;
using FlightSearch.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace FlightSearch.Tests;

public class FlightsControllerTests
{
    private readonly IFlightService _flightService = Substitute.For<IFlightService>();
    private readonly FlightsController _controller;

    public FlightsControllerTests()
    {
        var validator = new FlightSearchRequestValidator();
        _controller = new FlightsController(_flightService, validator);
    }

    [Fact]
    public async Task Search_ReturnsBadRequestWhenOriginMissing()
    {
        var request = new FlightSearchRequest("", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "oneway");
        var result = await _controller.Search(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Search_ReturnsBadRequestWhenDestinationMissing()
    {
        var request = new FlightSearchRequest("JFK", "", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "oneway");
        var result = await _controller.Search(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Search_ReturnsBadRequestForInvalidPassengerCount()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 0, "oneway");
        var result = await _controller.Search(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Search_ReturnsBadRequestForMissingReturnDate()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "return");
        var result = await _controller.Search(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Search_ReturnsOkWithFlights()
    {
        var depDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var request = new FlightSearchRequest("JFK", "LON", depDate, null, 1, "oneway");
        var flights = new List<FlightDto>
        {
            new("FS0001", "JFK", "LON", DateTime.Today.AddDays(1), DateTime.Today.AddDays(1).AddHours(8), 450m, 120)
        };
        _flightService.SearchAsync(request).Returns(flights);

        var result = await _controller.Search(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(flights, ok.Value);
    }
}
