using FluentValidation;

namespace ShipCapstone.Application.Features.Ports.Command.UpdatePort;

public class UpdatePortCommandValidator : AbstractValidator<UpdatePortCommand>
{
    public UpdatePortCommandValidator()
    {
        RuleFor(x => x.PortId)
            .NotNull().WithMessage("Id cảng không được để trống")
            .NotEmpty().WithMessage("Id cảng không được để trống")
            .NotEqual(Guid.Empty).WithMessage("Id cảng không hợp lệ");
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên của cảng không được để trống")
                .MaximumLength(255).WithMessage("Tên của cảng không được vượt quá 255 ký tự");
        });
        When(x => x.Country != null, () =>
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Quốc gia không được để trống")
                .MaximumLength(100).WithMessage("Quốc gia không được vượt quá 100 ký tự");
        });
        When(x => x.City != null, () =>
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Thành phố không được để trống")
                .MaximumLength(100).WithMessage("Thành phố không được vượt quá 100 ký tự");
        });
        When(x => x.Longitude != null, () =>
        {
            RuleFor(x => x.Longitude)
                .NotEmpty().WithMessage("Kinh độ không được để trống")
                .MaximumLength(20).WithMessage("Kinh độ không được vượt quá 20 ký tự");
        });
        When(x => x.Latitude != null, () =>
        {
            RuleFor(x => x.Latitude)
                .NotEmpty().WithMessage("Vĩ độ không được để trống")
                .MaximumLength(20).WithMessage("Vĩ độ không được vượt quá 20 ký tự");
        });
    }
}