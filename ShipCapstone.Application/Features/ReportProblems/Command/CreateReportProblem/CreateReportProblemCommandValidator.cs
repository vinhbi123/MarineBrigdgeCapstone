using FluentValidation;

namespace ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem;

public class CreateReportProblemCommandValidator : AbstractValidator<CreateReportProblemCommand>
{
    public CreateReportProblemCommandValidator()
    {
        RuleFor(rpp => rpp.PortId)
            .NotEmpty().WithMessage("Cảng không được để trống")
            .NotNull().WithMessage("Cảng không được để trống");
        RuleFor(rpp => rpp.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống")
            .NotNull().WithMessage("Tiêu đề không được để trống");
        RuleFor(rpp => rpp.Description)
            .NotEmpty().WithMessage("Mô tả không được để trống")
            .NotNull().WithMessage("Mô tả không được để trống");
    }
}