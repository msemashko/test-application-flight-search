using FlightSearch.Application.DTOs;
using FluentValidation;

namespace FlightSearch.Application.Validators;

public class DestinationsRequestValidator : AbstractValidator<DestinationsRequest>
{
    public DestinationsRequestValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin is required.")
            .Length(2, 4).WithMessage("Origin must be a 2-4 character airport code.");
    }
}
