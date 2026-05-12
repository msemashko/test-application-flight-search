using FlightSearch.Application.DTOs;
using FlightSearch.Application.Interfaces;
using FlightSearch.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.Infrastructure.Services;

public class FlightService(FlightSearchDbContext db) : IFlightService
{
    public async Task<List<FlightDto>> SearchAsync(FlightSearchRequest request)
    {
        var normalizedOrigin = request.Origin.ToUpperInvariant();
        var normalizedDest = request.Destination.ToUpperInvariant();

        var query = db.Flights
            .Where(f =>
                f.OriginCode == normalizedOrigin &&
                f.DestinationCode == normalizedDest &&
                f.AvailableSeats >= request.Passengers);

        if (request.DepartureDate.HasValue)
        {
            var depStart = request.DepartureDate.Value.ToDateTime(TimeOnly.MinValue);
            var depEnd = request.DepartureDate.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(f => f.DepartureTime >= depStart && f.DepartureTime <= depEnd);
        }

        return await query
            .OrderBy(f => f.DepartureTime)
            .Select(f => new FlightDto(
                f.FlightNumber,
                f.OriginCode,
                f.DestinationCode,
                f.DepartureTime,
                f.ArrivalTime,
                f.Price,
                f.AvailableSeats))
            .ToListAsync();
    }
}
