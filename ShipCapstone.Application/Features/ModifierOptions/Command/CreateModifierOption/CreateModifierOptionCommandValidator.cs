using FluentValidation;

namespace ShipCapstone.Application.Features.ModifierOptions.Command.CreateModifierOption;

public class CreateModifierOptionCommandValidator : AbstractValidator<CreateModifierOptionCommand>
{
    public CreateModifierOptionCommandValidator()
    {
        RuleFor(x => x.ModifierGroupId)
            .NotEmpty().WithMessage("Id nhóm tùy chọn không được để trống.")
            .NotNull().WithMessage("Id nhóm tùy chọn không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id nhóm tùy chọn không hợp lệ.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên tùy chọn không được để trống.")
            .NotNull().WithMessage("Tên tùy chọn không được để trống.")
            .MaximumLength(255).WithMessage("Tên tùy chọn không được vượt quá 255 ký tự.");
        
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải lớn hơn hoặc bằng 0.");
    }
}