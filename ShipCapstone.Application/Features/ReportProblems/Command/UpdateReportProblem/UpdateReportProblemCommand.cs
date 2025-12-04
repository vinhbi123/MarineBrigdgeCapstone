using Mediator;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ReportProblems.Command.UpdateReportProblem;

public class UpdateReportProblemCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public EReportProblemStatus? Status { get; set; }
}

public class UpdateReportProblemRequest
{
    public EReportProblemStatus? Status { get; set; }
}