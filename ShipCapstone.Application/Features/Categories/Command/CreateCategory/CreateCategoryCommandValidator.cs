using FluentValidation;

namespace ShipCapstone.Application.Features.Categories.Command.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên danh mục không được để trống")
            .NotNull().WithMessage("Tên danh mục không được để trống")
            .MaximumLength(255).WithMessage("Tên danh mục không được vượt quá 255 ký tự");
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