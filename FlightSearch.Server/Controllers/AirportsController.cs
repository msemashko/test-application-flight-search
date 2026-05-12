using FlightSearch.Application.DTOs;
using FlightSearch.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController(
    IAirportService airportService,
    IValidator<DestinationsRequest> destinationsValidator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var airports = await airportService.GetAllAsync();
        return Ok(airports);
    }

    [HttpGet("destinations/{origin}")]
    public async Task<IActionResult> GetDestinations(string origin)
    {
        var request = new DestinationsRequest(origin);
        var validation = await destinationsValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(new { error = validation.Errors[0].ErrorMessage });

        var destinations = await airportService.GetDestinationsAsync(origin);
        if (destinations is null)
            return NotFound(new { error = $"No routes found for origin '{origin.ToUpperInvariant()}'." });

        return Ok(destinations);
    }
}
