using FluentValidation;

namespace ShipCapstone.Application.Features.Products.Command.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId không được để trống");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Tên sản phẩm không được để trống")
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
            .MaximumLength(200).WithMessage("Tên sản phẩm không được vượt quá 200 ký tự");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId không được để trống");

        When(x => !x.IsHasVariant && x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price.Value)
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0");
        });

        When(x => x.IsHasVariant, () =>
        {
            RuleFor(x => x.Variants)
                .NotNull().WithMessage("Variants không được để trống khi IsHasVariant = true")
                .Must(v => v != null && v.Count > 0).WithMessage("Phải có ít nhất 1 variant");

            RuleForEach(x => x.Variants).ChildRules(v =>
            {
                v.RuleFor(r => r.Name)
                    .NotNull().WithMessage("Tên variant không được để trống")
                    .NotEmpty().WithMessage("Tên variant không được để trống");

                v.RuleFor(r => r.Price)
                    .GreaterThan(0).WithMessage("Giá variant phải lớn hơn 0");
            });
        });

        When(x => x.Images != null && x.Images.Count > 0, () =>
        {
            RuleForEach(x => x.Images).ChildRules(img =>
            {
                img.RuleFor(i => i.Url)
                    .NotNull().WithMessage("Url ảnh không được để trống")
                    .NotEmpty().WithMessage("Url ảnh không được để trống");
            });
        });
    }
}
