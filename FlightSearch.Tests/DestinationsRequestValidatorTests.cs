using FlightSearch.Application.DTOs;
using FlightSearch.Application.Validators;

namespace FlightSearch.Tests;

public class DestinationsRequestValidatorTests
{
    private readonly DestinationsRequestValidator _validator = new();

    [Fact]
    public void ValidOrigin_Passes()
    {
        var result = _validator.Validate(new DestinationsRequest("JFK"));
        Assert.True(result.IsValid);
    }

    [Fact]
    public void EmptyOrigin_Fails()
    {
        var result = _validator.Validate(new DestinationsRequest(""));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void SingleCharOrigin_Fails()
    {
        var result = _validator.Validate(new DestinationsRequest("X"));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void FiveCharOrigin_Fails()
    {
        var result = _validator.Validate(new DestinationsRequest("ABCDE"));
        Assert.False(result.IsValid);
    }
}
