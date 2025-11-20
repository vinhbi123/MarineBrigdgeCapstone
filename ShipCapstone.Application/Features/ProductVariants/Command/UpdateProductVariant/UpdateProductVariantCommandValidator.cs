using FluentValidation;

namespace ShipCapstone.Application.Features.ProductVariants.Command.UpdateProductVariant;

public class UpdateProductVariantCommandValidator : AbstractValidator<UpdateProductVariantCommand>
{
    public UpdateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductVariantId)
            .NotEmpty().WithMessage("Id biến thể sản phẩm không được để trống.")
            .NotNull().WithMessage("Id biến thể sản phẩm không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id biến thể sản phẩm không hợp lệ.");
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên biến thể sản phẩm không được để trống.")
                .MaximumLength(255).WithMessage("Tên biến thể sản phẩm không được vượt quá 255 ký tự.");
        });
        When(x => x.Price != null, () =>
        {
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá biến thể sản phẩm phải lớn hơn hoặc bằng 0.");
        });
    }
}