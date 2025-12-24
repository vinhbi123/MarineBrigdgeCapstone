using FluentValidation;

namespace ShipCapstone.Application.Features.Accounts.Command.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(a => a.OldPassword)
            .NotEmpty().WithMessage("Mật khẩu cũ không được để trống")
            .NotNull().WithMessage("Mật khẩu cũ không được để trống");
        RuleFor(a => a.NewPassword)
            .NotEmpty().WithMessage("Mật khẩu mới không được để trống")
            .NotNull().WithMessage("Mật khẩu mới không được để trống");
    }
}