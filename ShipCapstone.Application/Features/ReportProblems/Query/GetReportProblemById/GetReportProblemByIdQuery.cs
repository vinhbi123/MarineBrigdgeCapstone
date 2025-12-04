using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ReportProblems.Query.GetReportProblemById;

public class GetReportProblemByIdQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}