using FluentValidation;

namespace ShipCapstone.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private static readonly string[] _allowedImageExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
            .NotNull().WithMessage("Tên sản phẩm không được để trống.")
            .MaximumLength(255).WithMessage("Tên sản phẩm không được vượt quá 255 ký tự.");
        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả sản phẩm không được để trống.")
                .MaximumLength(500).WithMessage("Mô tả sản phẩm không được vượt quá 500 ký tự.");
        });
        RuleFor(x => x.CategoryId)
            .NotNull().WithMessage("Danh mục sản phẩm không được để trống.");
        When(x => x.IsHasVariant, () =>
        {
            RuleFor(x => x.ProductVariants)
                .NotNull().WithMessage("Sản phẩm có biến thể phải có ít nhất một biến thể.")
                .Must(variants => variants != null && variants.Count > 0)
                .WithMessage("Sản phẩm có biến thể phải có ít nhất một biến thể.");
            RuleForEach(x => x.ProductVariants).ChildRules(variant =>
            {
                variant.RuleFor(v => v.Name)
                    .NotEmpty().WithMessage("Tên biến thể không được để trống.")
                    .NotNull().WithMessage("Tên biến thể không được để trống.")
                    .MaximumLength(255).WithMessage("Tên biến thể không được vượt quá 255 ký tự.");
                variant.RuleFor(v => v.Price)
                    .GreaterThanOrEqualTo(0).WithMessage("Giá biến thể phải lớn hơn hoặc bằng 0.");
            });
        });
        When(x => !x.IsHasVariant, () =>
        {
            RuleFor(x => x.Price)
                .NotNull().WithMessage("Giá sản phẩm không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Giá sản phẩm phải lớn hơn hoặc bằng 0.");
        });
        RuleForEach(x => x.ProductImages)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedImageExtensions.Contains(extension);
            }).WithMessage("Hình ảnh không hợp lý với các định dạng: " +
                           string.Join(", ", _allowedImageExtensions));
    }
}