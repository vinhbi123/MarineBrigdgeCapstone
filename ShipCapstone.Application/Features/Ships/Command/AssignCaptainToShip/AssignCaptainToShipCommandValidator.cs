using FluentValidation;

namespace ShipCapstone.Application.Features.Ships.Command.AssignCaptainToShip;

public class AssignCaptainToShipCommandValidator : AbstractValidator<AssignCaptainToShipCommand>
{
    public AssignCaptainToShipCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email thuyền trưởng không được để trống")
            .NotNull().WithMessage("Email thuyền trưởng không được để trống");
    }
}