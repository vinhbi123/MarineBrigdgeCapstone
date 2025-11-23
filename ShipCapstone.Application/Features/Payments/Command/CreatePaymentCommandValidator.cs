using FluentValidation;

namespace ShipCapstone.Application.Features.Payments.Command;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống")
            .NotNull().WithMessage("Địa chỉ không được để trống");
    }
}