using FlightSearch.Application.DTOs;
using FlightSearch.Application.Interfaces;
using FlightSearch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.Infrastructure.Services;

public class AirportService(FlightSearchDbContext db) : IAirportService
{
    public async Task<List<AirportDto>> GetAllAsync()
    {
        return await db.Airports
            .OrderBy(a => a.Code)
            .Select(a => new AirportDto(a.Code, a.Name, a.City, a.Country))
            .ToListAsync();
    }

    public async Task<List<AirportDto>?> GetDestinationsAsync(string originCode)
    {
        var normalized = originCode.ToUpperInvariant();

        var originExists = await db.Airports.AnyAsync(a => a.Code == normalized);
        if (!originExists)
            return null;

        return await db.Routes
            .Where(r => r.OriginCode == normalized)
            .Select(r => new AirportDto(
                r.Destination.Code,
                r.Destination.Name,
                r.Destination.City,
                r.Destination.Country))
            .ToListAsync();
    }
}
