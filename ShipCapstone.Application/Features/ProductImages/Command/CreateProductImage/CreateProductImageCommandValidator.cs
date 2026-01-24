using FluentValidation;

namespace ShipCapstone.Application.Features.ProductImages.Command.CreateProductImage;

public class CreateProductImageCommandValidator : AbstractValidator<CreateProductImageCommand>
{
    private static readonly string[] _allowedImageExtensions = new[]
    {
        ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp"
    };
    
    public CreateProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("Id của sản phẩm không được bỏ trống")
            .NotEmpty().WithMessage("Id của sản phẩm không được bỏ trống")
            .NotEqual(Guid.Empty).WithMessage("Id của sản phẩm không được bỏ trống");
        
        RuleFor(x => x.Image)
            .Cascade(CascadeMode.Stop)
            .Must(file =>
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                return _allowedImageExtensions.Contains(extension);
            }).WithMessage("Hình ảnh không hợp lý với các định dạng: " +
                           string.Join(", ", _allowedImageExtensions));
        When(x => x.SortOrder != null , () =>
        {
            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Thứ tự sắp xếp phải lớn hơn hoặc bằng 0");
        });
            
    }
}