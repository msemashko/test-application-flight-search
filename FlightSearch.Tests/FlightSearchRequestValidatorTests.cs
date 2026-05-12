using FlightSearch.Application.DTOs;
using FlightSearch.Application.Validators;

namespace FlightSearch.Tests;

public class FlightSearchRequestValidatorTests
{
    private readonly FlightSearchRequestValidator _validator = new();

    [Fact]
    public void ValidRequest_Passes()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 2, "oneway");
        var result = _validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void EmptyOrigin_Fails()
    {
        var request = new FlightSearchRequest("", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "oneway");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Origin");
    }

    [Fact]
    public void EmptyDestination_Fails()
    {
        var request = new FlightSearchRequest("JFK", "", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "oneway");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Destination");
    }

    [Fact]
    public void PassengerCountZero_Fails()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 0, "oneway");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Passengers");
    }

    [Fact]
    public void PassengerCountTen_Fails()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 10, "oneway");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void InvalidTripType_Fails()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "multi");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "TripType");
    }

    [Fact]
    public void ReturnTrip_MissingReturnDate_Fails()
    {
        var request = new FlightSearchRequest("JFK", "LON", DateOnly.FromDateTime(DateTime.Today.AddDays(1)), null, 1, "return");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ReturnDate");
    }

    [Fact]
    public void ReturnTrip_ReturnDateBeforeDeparture_Fails()
    {
        var dep = DateOnly.FromDateTime(DateTime.Today.AddDays(3));
        var ret = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var request = new FlightSearchRequest("JFK", "LON", dep, ret, 1, "return");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ReturnDate");
    }

    [Fact]
    public void ReturnTrip_ValidReturnDate_Passes()
    {
        var dep = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var ret = DateOnly.FromDateTime(DateTime.Today.AddDays(3));
        var request = new FlightSearchRequest("JFK", "LON", dep, ret, 1, "return");
        var result = _validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Fact]
    public void MissingDepartureDate_Fails()
    {
        var request = new FlightSearchRequest("JFK", "LON", null, null, 1, "oneway");
        var result = _validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DepartureDate");
    }
}
