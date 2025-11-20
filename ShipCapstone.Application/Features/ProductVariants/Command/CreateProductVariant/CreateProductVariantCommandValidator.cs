using FluentValidation;

namespace ShipCapstone.Application.Features.ProductVariants.Command.CreateProductVariant;

public class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Id sản phẩm không được để trống.")
            .NotNull().WithMessage("Id sản phẩm không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id sản phẩm không hợp lệ.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên biến thể không được để trống.")
            .NotNull().WithMessage("Tên biến thể không được để trống.")
            .MaximumLength(255).WithMessage("Tên biến thể không được vượt quá 255 ký tự.");
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Giá biến thể phải lớn hơn hoặc bằng 0.");
    }
}