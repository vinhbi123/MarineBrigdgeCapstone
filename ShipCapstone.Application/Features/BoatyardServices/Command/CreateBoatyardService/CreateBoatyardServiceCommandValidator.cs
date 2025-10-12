using FluentValidation;

namespace ShipCapstone.Application.Features.BoatyardServices.Command.CreateBoatyardService;

public class CreateBoatyardServiceCommandValidator : AbstractValidator<CreateBoatyardServiceCommand>
{
    public CreateBoatyardServiceCommandValidator()
    {
        RuleFor(x => x.TypeService)
            .NotNull().WithMessage("Loại dịch vụ không được để trống")
            .NotEmpty().WithMessage("Loại dịch vụ không được để trống")
            .MaximumLength(100).WithMessage("Loại dịch vụ không được vượt quá 100 ký tự");
        
        RuleFor(x => x.Price)
            .NotNull().WithMessage("Giá dịch vụ không được để trống")
            .GreaterThan(0).WithMessage("Giá dịch vụ phải lớn hơn 0");
    }
}