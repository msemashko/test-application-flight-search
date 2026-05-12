namespace FlightSearch.Application.DTOs;

public record FlightDto(
    string FlightNumber,
    string Origin,
    string Destination,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    decimal Price,
    int AvailableSeats);
