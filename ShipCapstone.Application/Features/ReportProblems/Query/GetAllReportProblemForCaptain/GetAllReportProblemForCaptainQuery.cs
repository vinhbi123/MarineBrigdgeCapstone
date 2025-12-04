using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ReportProblems.Query.GetAllReportProblemForCaptain;

public class GetAllReportProblemForCaptainQuery : IRequest<ApiResponse>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public bool IsAsc { get; set; }
}