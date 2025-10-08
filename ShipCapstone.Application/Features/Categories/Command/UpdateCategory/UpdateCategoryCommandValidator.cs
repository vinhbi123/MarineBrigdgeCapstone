using FluentValidation;

namespace ShipCapstone.Application.Features.Categories.Command.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Id danh mục không được để trống")
            .NotNull().WithMessage("Id danh mục không được để trống")
            .NotEqual(Guid.Empty).WithMessage("Id danh mục không hợp lệ");
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên danh mục không được để trống")
                .MaximumLength(255).WithMessage("Tên danh mục không được vượt quá 255 ký tự");
        });
        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả danh mục không được để trống")
                .MaximumLength(1000).WithMessage("Mô tả danh mục không được vượt quá 1000 ký tự");
        });
        When(x => x.Image != null, () =>
        {
            var allowedImageExtensions = new[]
            {
                ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
            };
            RuleFor(x => x.Image)
                .Cascade(CascadeMode.Stop)
                .Must(file =>
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                    return allowedImageExtensions.Contains(extension);
                }).WithMessage("Ảnh danh mục không hợp lý với các định dạng: " + string.Join(", ", allowedImageExtensions));
        });
    }
}