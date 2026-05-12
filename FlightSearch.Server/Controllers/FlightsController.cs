using FlightSearch.Application.DTOs;
using FlightSearch.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController(
    IFlightService flightService,
    IValidator<FlightSearchRequest> searchValidator) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] FlightSearchRequest request)
    {
        var validation = await searchValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(new { error = validation.Errors[0].ErrorMessage });

        var results = await flightService.SearchAsync(request);
        return Ok(results);
    }
}
