using FluentValidation;

namespace ShipCapstone.Application.Features.Authentication.Command.SendOtp;

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống")
            .NotNull().WithMessage("Email không được để trống")
            .EmailAddress().WithMessage("Email không hợp lệ")
            .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự");
    }
}