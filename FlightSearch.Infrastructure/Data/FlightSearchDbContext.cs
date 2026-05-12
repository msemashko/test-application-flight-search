using FlightSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightSearch.Infrastructure.Data;

public class FlightSearchDbContext(DbContextOptions<FlightSearchDbContext> options) : DbContext(options)
{
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Flight> Flights => Set<Flight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(e =>
        {
            e.HasKey(a => a.Code);
            e.Property(a => a.Code).HasMaxLength(4);
        });

        modelBuilder.Entity<Route>(e =>
        {
            e.HasOne(r => r.Origin)
                .WithMany(a => a.RoutesFromHere)
                .HasForeignKey(r => r.OriginCode)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Destination)
                .WithMany()
                .HasForeignKey(r => r.DestinationCode)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(r => new { r.OriginCode, r.DestinationCode }).IsUnique();
        });

        modelBuilder.Entity<Flight>(e =>
        {
            e.HasOne(f => f.Origin)
                .WithMany()
                .HasForeignKey(f => f.OriginCode)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(f => f.Destination)
                .WithMany()
                .HasForeignKey(f => f.DestinationCode)
                .OnDelete(DeleteBehavior.Restrict);

            e.Property(f => f.Price).HasColumnType("decimal(10,2)");
        });
    }
}
