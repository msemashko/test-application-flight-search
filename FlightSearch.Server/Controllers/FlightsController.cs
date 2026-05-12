using FlightSearch.Application.DTOs;
using FlightSearch.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController(
    IFlightService flightService,
    IValidator<FlightSearchRequest> searchValidator,
    ILogger<FlightsController> logger) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] FlightSearchRequest request)
    {
        var validation = await searchValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            logger.LogWarning("Invalid search request: {Error}", validation.Errors[0].ErrorMessage);
            return BadRequest(new { error = validation.Errors[0].ErrorMessage });
        }

        var results = await flightService.SearchAsync(request);
        logger.LogInformation(
            "Search {Origin} -> {Destination} on {Date}, {Passengers} pax: {Count} flights found",
            request.Origin, request.Destination, request.DepartureDate, request.Passengers, results.Count);
        return Ok(results);
    }
}
