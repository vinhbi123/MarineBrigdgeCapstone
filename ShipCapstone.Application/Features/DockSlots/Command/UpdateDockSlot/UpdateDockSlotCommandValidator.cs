using FluentValidation;

namespace ShipCapstone.Application.Features.DockSlots.Command.UpdateDockSlot;

public class UpdateDockSlotCommandValidator : AbstractValidator<UpdateDockSlotCommand>
{
    public UpdateDockSlotCommandValidator()
    {
        RuleFor(x => x.DockSlotId)
            .NotNull().WithMessage("Id chỗ neo đậu không được để trống")
            .NotEmpty().WithMessage("Id chỗ neo đậu không được để trống")
            .NotEqual(Guid.Empty).WithMessage("Id chỗ neo đậu không hợp lệ");
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên chỗ neo đậu không được để trống")
                .MaximumLength(255).WithMessage("Tên chỗ neo đậu không được vượt quá 255 ký tự");
        });
        RuleFor(x => x.AssignedFrom)
            .NotNull().WithMessage("Thời gian bắt đầu không được để trống");
        When(x => x.AssignedUntil != null, () =>
        {
            RuleFor(x => x.AssignedUntil)
                .GreaterThan(x => x.AssignedFrom).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
        });
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("Trạng thái chỗ neo đậu không được để trống");
    }
}