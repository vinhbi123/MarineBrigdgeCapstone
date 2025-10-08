using FluentValidation;

namespace ShipCapstone.Application.Features.ModifierGroups.Command.UpdateModifierGroup;

public class UpdateModifierGroupCommandValidator : AbstractValidator<UpdateModifierGroupCommand>
{
    public UpdateModifierGroupCommandValidator()
    {
        RuleFor(x => x.ModifierGroupId)
            .NotEmpty().WithMessage("Id của nhóm tùy chỉnh không được để trống.")
            .NotNull().WithMessage("Id của nhóm tùy chỉnh không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id của nhóm tùy chỉnh không hợp lệ.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên nhóm tùy chỉnh không được để trống.")
            .NotNull().WithMessage("Tên nhóm tùy chỉnh không được để trống.")
            .MaximumLength(255).WithMessage("Tên nhóm tùy chỉnh không được vượt quá 255 ký tự.");
    }
}