using FluentValidation;
using ShipCapstone.Domain.Models.ReportProblemCommand;

namespace ShipCapstone.Application.Features.Reports.Command.CreateReportProblem;

public class CreateReportProblemCommandValidator : AbstractValidator<CreateReportProblemCommand>
{
    public CreateReportProblemCommandValidator()
    {
        RuleFor(x => x.PortId)
            .NotEmpty().WithMessage("PortId không được để trống");

        RuleFor(x => x.ShipId)
            .NotEmpty().WithMessage("ShipId không được để trống");

        RuleFor(x => x.Title)
            .NotNull().WithMessage("Tiêu đề không được để trống")
            .NotEmpty().WithMessage("Tiêu đề không được để trống")
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự");

        RuleFor(x => x.Description)
            .NotNull().WithMessage("Mô tả không được để trống")
            .NotEmpty().WithMessage("Mô tả không được để trống")
            .MaximumLength(2000).WithMessage("Mô tả không được vượt quá 2000 ký tự");
    }
}
