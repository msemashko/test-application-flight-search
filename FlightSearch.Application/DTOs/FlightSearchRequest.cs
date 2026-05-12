namespace FlightSearch.Application.DTOs;

public record FlightSearchRequest(
    string Origin,
    string Destination,
    DateOnly? DepartureDate,
    DateOnly? ReturnDate,
    int Passengers,
    string TripType);
