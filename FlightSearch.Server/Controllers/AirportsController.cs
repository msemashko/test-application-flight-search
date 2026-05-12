using FlightSearch.Application.DTOs;
using FlightSearch.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController(
    IAirportService airportService,
    IValidator<DestinationsRequest> destinationsValidator,
    ILogger<AirportsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var airports = await airportService.GetAllAsync();
        logger.LogInformation("Returned {Count} airports", airports.Count);
        return Ok(airports);
    }

    [HttpGet("destinations/{origin}")]
    public async Task<IActionResult> GetDestinations(string origin)
    {
        var request = new DestinationsRequest(origin);
        var validation = await destinationsValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            logger.LogWarning("Invalid destination request for origin '{Origin}': {Error}", origin, validation.Errors[0].ErrorMessage);
            return BadRequest(new { error = validation.Errors[0].ErrorMessage });
        }

        var destinations = await airportService.GetDestinationsAsync(origin);
        if (destinations is null)
        {
            logger.LogWarning("No routes found for origin '{Origin}'", origin.ToUpperInvariant());
            return NotFound(new { error = $"No routes found for origin '{origin.ToUpperInvariant()}'." });
        }

        logger.LogInformation("Origin '{Origin}' returned {Count} destinations", origin.ToUpperInvariant(), destinations.Count);
        return Ok(destinations);
    }
}
