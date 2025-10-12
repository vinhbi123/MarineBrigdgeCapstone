using FluentValidation;

namespace ShipCapstone.Application.Features.BoatyardServices.Command.UpdateBoatyardService;

public class UpdateBoatyardServiceCommandValidator : AbstractValidator<UpdateBoatyardServiceCommand>
{
    public UpdateBoatyardServiceCommandValidator()
    {
        RuleFor(x => x.BoatyardServiceId)
            .NotEmpty().WithMessage("Id dịch vụ xưởng không được để trống")
            .NotNull().WithMessage("Id dịch vụ xưởng không được để trống")
            .NotEqual(Guid.Empty).WithMessage("Id dịch vụ xưởng không hợp lệ");
        When(x => x.TypeService != null, () =>
        {
            RuleFor(x => x.TypeService)
                .NotEmpty().WithMessage("Loại dịch vụ không được để trống")
                .MaximumLength(100).WithMessage("Loại dịch vụ không được vượt quá 100 ký tự");
        });
        When(x => x.Price != null, () =>
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Giá dịch vụ phải lớn hơn 0");
        });
    }
}