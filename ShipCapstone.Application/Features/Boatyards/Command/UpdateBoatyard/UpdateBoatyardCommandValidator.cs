using FluentValidation;

namespace ShipCapstone.Application.Features.Boatyards.Command.UpdateBoatyard;

public class UpdateBoatyardCommandValidator : AbstractValidator<UpdateBoatyardCommand>
{
    public UpdateBoatyardCommandValidator()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên bến không được để trống")
                .MaximumLength(255).WithMessage("Tên bến không được vượt quá 255 ký tự");
        });
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
        When(x => x.FullName != null, () =>
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên không được để trống")
                .MaximumLength(255).WithMessage("Họ và tên không được vượt quá 255 ký tự");
        });
        When(x => x.Password != null, () =>
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
                .MaximumLength(20).WithMessage("Mật khẩu không vượt quá 20 ký tự");
        });
        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Địa chỉ không được để trống")
                .MaximumLength(500).WithMessage("Địa chỉ không được vượt quá 500 ký tự");
        });
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

    }
}