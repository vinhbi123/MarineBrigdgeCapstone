using FluentValidation;

namespace ShipCapstone.Application.Features.DockSlots.Command.CreateDockSlot;

public class CreateDockSlotCommandValidator : AbstractValidator<CreateDockSlotCommand>
{
    public CreateDockSlotCommandValidator()
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