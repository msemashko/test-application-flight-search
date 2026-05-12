namespace FlightSearch.Domain.Entities;

public class Airport
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public List<Route> RoutesFromHere { get; set; } = [];
}
