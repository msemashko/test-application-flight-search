using FlightSearch.Application.DTOs;

namespace FlightSearch.Application.Interfaces;

public interface IAirportService
{
    Task<List<AirportDto>> GetAllAsync();
    Task<List<AirportDto>?> GetDestinationsAsync(string originCode);
}
