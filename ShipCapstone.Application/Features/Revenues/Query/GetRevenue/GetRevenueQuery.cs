using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Revenues.Query.GetRevenue;

public class GetRevenueQuery : IRequest<ApiResponse>
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}