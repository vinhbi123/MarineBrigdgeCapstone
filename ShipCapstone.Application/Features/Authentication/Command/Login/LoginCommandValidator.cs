using FluentValidation;

namespace ShipCapstone.Application.Features.Authentication.Command.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Tên đăng nhập hoặc Email không được để trống")
            .NotNull().WithMessage("Tên đăng nhập hoặc Email không được để trống")
            .MaximumLength(255).WithMessage("Tên đăng nhập hoặc Email không được vượt quá 255 ký tự");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .NotNull().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .MaximumLength(20).WithMessage("Mật khẩu không vượt quá 20 ký tự");
    }
}