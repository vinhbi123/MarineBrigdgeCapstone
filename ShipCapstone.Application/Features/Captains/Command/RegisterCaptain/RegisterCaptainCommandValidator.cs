using FluentValidation;

namespace ShipCapstone.Application.Features.Captains.Command.RegisterCaptain;

public class RegisterCaptainCommandValidator : AbstractValidator<RegisterCaptainCommand>
{
    public RegisterCaptainCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ và tên không được để trống")
            .NotNull().WithMessage("Họ và tên không được để trống")
            .MaximumLength(255).WithMessage("Họ và tên không được vượt quá 255 ký tự");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống")
            .NotNull().WithMessage("Email không được để trống")
            .EmailAddress().WithMessage("Email không đúng định dạng")
            .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
            .NotNull().WithMessage("Tên đăng nhập không được để trống")
            .MaximumLength(20).WithMessage("Tên đăng nhập không được vượt quá 20 ký tự");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .NotNull().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .MaximumLength(20).WithMessage("Mật khẩu không vượt quá 20 ký tự");        
        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Địa chỉ không được để trống")
                .MaximumLength(500).WithMessage("Địa chỉ không được vượt quá 500 ký tự");
        });
        When(x => x.PhoneNumber != null, () =>
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại không được để trống")
                .MaximumLength(15).WithMessage("Số điện thoại không được vượt quá 15 ký tự");
        });
    }
}