using FluentValidation;

namespace ShipCapstone.Application.Features.Bookings.Command.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.ShipId).NotEmpty().WithMessage("ShipId is required.");
            RuleFor(x => x.DockSlotId).NotEmpty().WithMessage("DockSlotId is required.");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime is required.");

            When(x => x.EndTime.HasValue, () =>
            {
                RuleFor(x => x.EndTime)
                    .GreaterThan(x => x.StartTime)
                    .WithMessage("EndTime must be greater than StartTime.");
            });

            RuleFor(x => x.Services)
                .NotNull().WithMessage("Services cannot be null.");

            RuleForEach(x => x.Services)
                .NotEmpty().WithMessage("Service Id cannot be empty.");
        }
    }
}
