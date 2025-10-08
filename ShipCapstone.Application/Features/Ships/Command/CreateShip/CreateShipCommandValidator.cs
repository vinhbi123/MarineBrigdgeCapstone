using FluentValidation;

namespace ShipCapstone.Application.Features.Ships.Command.CreateShip;

public class CreateShipCommandValidator : AbstractValidator<CreateShipCommand>
{
    public CreateShipCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên tàu không được để trống")
            .NotNull().WithMessage("Tên tàu không được để trống")
            .MaximumLength(255).WithMessage("Tên tàu không được vượt quá 255 ký tự");

        When(x => x.ImoNumber != null, () =>
        {
            RuleFor(x => x.ImoNumber)
                .NotEmpty().WithMessage("Số IMO không được để trống")
                .MaximumLength(100).WithMessage("Số IMO không được vượt quá 100 ký tự");
        });
        When(x => x.RegisterNo != null, () =>
        {
            RuleFor(x => x.RegisterNo)
                .NotEmpty().WithMessage("Số đăng ký không được để trống")
                .MaximumLength(100).WithMessage("Số đăng ký không được vượt quá 100 ký tự");
        });
        When(x => x.BuildYear != null, () =>
        {
            RuleFor(x => x.BuildYear)
                .GreaterThan(0).WithMessage("Năm xây dựng phải lớn hơn 0")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Năm xây dựng phải nhỏ hơn hoặc bằng năm hiện tại");
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