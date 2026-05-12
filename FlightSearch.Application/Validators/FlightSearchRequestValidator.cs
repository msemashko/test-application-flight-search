using FlightSearch.Application.DTOs;
using FluentValidation;

namespace FlightSearch.Application.Validators;

public class FlightSearchRequestValidator : AbstractValidator<FlightSearchRequest>
{
    public FlightSearchRequestValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin is required.")
            .Length(2, 4).WithMessage("Origin must be a 2-4 character airport code.");

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("Destination is required.")
            .Length(2, 4).WithMessage("Destination must be a 2-4 character airport code.");

        RuleFor(x => x.Passengers)
            .InclusiveBetween(1, 9).WithMessage("Passenger count must be between 1 and 9.");

        RuleFor(x => x.TripType)
            .Must(t => t is "oneway" or "return").WithMessage("Trip type must be 'oneway' or 'return'.");

        RuleFor(x => x.DepartureDate)
            .NotNull().WithMessage("Departure date is required.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Departure date cannot be in the past.");

        When(x => x.TripType == "return", () =>
        {
            RuleFor(x => x.ReturnDate)
                .NotNull().WithMessage("Return date is required for return trips.")
                .GreaterThanOrEqualTo(x => x.DepartureDate)
                .WithMessage("Return date must be on or after departure date.");
        });
    }
}
