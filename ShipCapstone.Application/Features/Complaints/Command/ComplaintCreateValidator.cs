using FluentValidation;

namespace ShipCapstone.Application.Features.Complaints.Command.CreateComplaint;

public class CreateComplaintCommandValidator : AbstractValidator<CreateComplaintCommand>
{
    public CreateComplaintCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung khiếu nại không được để trống");

        RuleFor(x => x)
            .Must(x => x.OrderId != null || x.BookingId != null)
            .WithMessage("Complaint phải liên quan đến Order hoặc Booking");

        RuleFor(x => x.ReceiverAccountId)
            .NotEmpty().WithMessage("ReceiverAccountId không được để trống");
    }
}
