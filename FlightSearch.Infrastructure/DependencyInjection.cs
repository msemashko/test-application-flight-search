using FlightSearch.Application.Interfaces;
using FlightSearch.Infrastructure.Data;
using FlightSearch.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlightSearch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FlightSearchDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IAirportService, AirportService>();
        services.AddScoped<IFlightService, FlightService>();

        return services;
    }
}
