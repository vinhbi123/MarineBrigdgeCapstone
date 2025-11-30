using FluentValidation;

namespace ShipCapstone.Application.Features.Reviews.Command.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(r => r.Rating)
            .NotNull().WithMessage("Đánh giá không được để trống")
            .InclusiveBetween(1, 5).WithMessage("Đánh giá phải lớn hơn 1 và nhỏ hơn 5");
    }
}