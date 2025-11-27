using FluentValidation;
using ShipCapstone.Application.Features.Ports.Command.UpdatePort;

namespace ShipCapstone.Application.Features.Products.Command.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Id sản phẩm không được để trống.")
            .NotNull().WithMessage("Id sản phẩm không được để trống.")
            .NotEqual(Guid.Empty).WithMessage("Id sản phẩm không hợp lệ.");
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
                .MaximumLength(255).WithMessage("Tên sản phẩm không được vượt quá 255 ký tự.");
        });
        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả sản phẩm không được để trống.")
                .MaximumLength(500).WithMessage("Mô tả sản phẩm không được vượt quá 500 ký tự.");
        });
        When(x => x.CategoryId != null, () =>
        {
            RuleFor(x => x.CategoryId)
                .NotEqual(Guid.Empty).WithMessage("Id danh mục sản phẩm không hợp lệ.");
        });
    }
}   