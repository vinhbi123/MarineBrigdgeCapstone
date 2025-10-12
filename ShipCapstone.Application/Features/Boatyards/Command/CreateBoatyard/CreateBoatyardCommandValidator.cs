using FluentValidation;

namespace ShipCapstone.Application.Features.Boatyards.Command.CreateBoatyard;

public class CreateBoatyardCommandValidator : AbstractValidator<CreateBoatyardCommand>
{
    public CreateBoatyardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Tên bến không được để trống")
            .NotEmpty().WithMessage("Tên bến không được để trống")
            .MaximumLength(255).WithMessage("Tên bến không được vượt quá 255 ký tự");
        When(x => x.Longitude != null, () =>
        {
            RuleFor(x => x.Longitude)
                .NotEmpty().WithMessage("Kinh độ không được để trống")
                .MaximumLength(50).WithMessage("Kinh độ không được vượt quá 50 ký tự");
        });
        When(x => x.Latitude != null, () =>
        {
            RuleFor(x => x.Latitude)
                .NotEmpty().WithMessage("Vĩ độ không được để trống")
                .MaximumLength(50).WithMessage("Vĩ độ không được vượt quá 50 ký tự");
        });
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
        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("Mã OTP không được để trống")
            .NotNull().WithMessage("Mã OTP không được để trống")
            .Length(4).WithMessage("Mã OTP phải có đúng 4 ký tự");
        When(x => x.Avatar != null, () =>
        {
            var allowedImageExtensions = new[]
            {
                ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
            };
            RuleFor(x => x.Avatar)
                .Cascade(CascadeMode.Stop)
                .Must(file =>
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                    return allowedImageExtensions.Contains(extension);
                }).WithMessage("Hình đại diện không hợp lý với các định dạng: " + string.Join(", ", allowedImageExtensions));
        });
        RuleForEach(x => x.DockSlots).SetValidator(new CreateDockSlotForCreateBoatyardValidator());
    }
}

public class CreateDockSlotForCreateBoatyardValidator : AbstractValidator<CreateDockSlotForCreateBoatyard>
{
    public CreateDockSlotForCreateBoatyardValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên chỗ neo đậu không được để trống")
            .NotNull().WithMessage("Tên chỗ neo đậu không được để trống")
            .MaximumLength(255).WithMessage("Tên chỗ neo đậu không được vượt quá 255 ký tự");
        RuleFor(x => x.AssignedFrom)
            .NotNull().WithMessage("Thời gian bắt đầu không được để trống");
        When(x => x.AssignedUntil != null, () =>
        {
            RuleFor(x => x.AssignedUntil)
                .GreaterThan(x => x.AssignedFrom).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
        });
    }
}