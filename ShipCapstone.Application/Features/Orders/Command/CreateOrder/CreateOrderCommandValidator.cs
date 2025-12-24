using FluentValidation;

namespace ShipCapstone.Application.Features.Orders.Command.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.OrderItems)
                .NotEmpty().WithMessage("Sản phẩm trong đơn hàng không được để trống")
                .NotNull().WithMessage("Sản phẩm trong đơn hàng không được để trống");
        }
    }
}
