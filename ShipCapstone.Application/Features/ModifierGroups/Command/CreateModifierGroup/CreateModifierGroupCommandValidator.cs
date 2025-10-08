using FluentValidation;

namespace ShipCapstone.Application.Features.ModifierGroups.Command.CreateModifierGroup;

public class CreateModifierGroupCommandValidator : AbstractValidator<CreateModifierGroupCommand>
{
    public CreateModifierGroupCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên nhóm tùy chọn không được để trống.")
            .NotNull().WithMessage("Tên nhóm tùy chọn không được để trống.")
            .MaximumLength(255).WithMessage("Tên nhóm tùy chọn không được vượt quá 255 ký tự.");
        RuleForEach(x => x.ModifierOptions).SetValidator(new CreateModifierOptionRequestValidator());
    }
}
public class CreateModifierOptionRequestValidator : AbstractValidator<CreateModifierOptionRequest>
{
    public CreateModifierOptionRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên tùy chọn không được để trống.")
            .NotNull().WithMessage("Tên tùy chọn không được để trống.")
            .MaximumLength(255).WithMessage("Tên tùy chọn không được vượt quá 255 ký tự.");
        
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải lớn hơn hoặc bằng 0.");
    }
}