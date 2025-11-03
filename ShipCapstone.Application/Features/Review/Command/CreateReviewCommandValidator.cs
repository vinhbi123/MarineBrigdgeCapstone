using FluentValidation;

namespace ShipCapstone.Application.Features.Reviews.Command.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId không được để trống");

            RuleFor(x => x.Rating)
                .NotNull().WithMessage("Rating không được để trống")
                .InclusiveBetween(1, 5).WithMessage("Rating phải trong khoảng 1 đến 5");

            RuleFor(x => x.Comment)
                .MaximumLength(1000).WithMessage("Bình luận không được vượt quá 1000 ký tự");
        }
    }
}
