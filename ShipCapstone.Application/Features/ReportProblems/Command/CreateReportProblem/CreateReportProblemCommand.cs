using Mediator;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ReportProblems.Command.CreateReportProblem;

public class CreateReportProblemCommand : IRequest<ApiResponse>
{
    public Guid PortId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}