using FlightSearch.Application.DTOs;

namespace FlightSearch.Application.Interfaces;

public interface IFlightService
{
    Task<List<FlightDto>> SearchAsync(FlightSearchRequest request);
}
