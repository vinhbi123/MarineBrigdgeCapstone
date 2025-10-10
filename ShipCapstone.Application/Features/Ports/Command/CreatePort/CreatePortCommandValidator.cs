using FluentValidation;

namespace ShipCapstone.Application.Features.Ports.Command.CreatePort;

public class CreatePortCommandValidator : AbstractValidator<CreatePortCommand>
{
    public CreatePortCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên của cảng không được để trống")
            .NotNull().WithMessage("Tên của cảng không được để trống")
            .MaximumLength(255).WithMessage("Tên của cảng không được vượt quá 255 ký tự");
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Quốc gia không được để trống")
            .NotNull().WithMessage("Quốc gia không được để trống")
            .MaximumLength(100).WithMessage("Quốc gia không được vượt quá 100 ký tự");
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