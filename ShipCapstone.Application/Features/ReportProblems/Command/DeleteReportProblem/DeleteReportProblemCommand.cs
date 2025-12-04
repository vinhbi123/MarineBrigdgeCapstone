using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ReportProblems.Command.DeleteReportProblem;

public class DeleteReportProblemCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}