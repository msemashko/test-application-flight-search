namespace FlightSearch.Domain.Entities;

public class Route
{
    public int Id { get; set; }
    public string OriginCode { get; set; } = string.Empty;
    public string DestinationCode { get; set; } = string.Empty;

    public Airport Origin { get; set; } = null!;
    public Airport Destination { get; set; } = null!;
}
