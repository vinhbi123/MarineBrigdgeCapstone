using FluentValidation;

namespace ShipCapstone.Application.Features.ModifierOptions.Command.UpdateModifierOption;

public class UpdateModifierOptionCommandValidator : AbstractValidator<UpdateModifierOptionCommand>
{
    public UpdateModifierOptionCommandValidator()
    {
        RuleFor(x => x.ModifierOptionId)
            .NotEmpty().WithMessage("Id tùy chọn không được để trống.")
            .NotNull().WithMessage("Id tùy chọn không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id tùy chọn không hợp lệ.");
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên tùy chọn không được để trống.")
                .MaximumLength(255).WithMessage("Tên tùy chọn không được vượt quá 255 ký tự.");
        });
        When(x => x.DisplayOrder != null, () =>
        {
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải lớn hơn hoặc bằng 0.");
        });
    }
}