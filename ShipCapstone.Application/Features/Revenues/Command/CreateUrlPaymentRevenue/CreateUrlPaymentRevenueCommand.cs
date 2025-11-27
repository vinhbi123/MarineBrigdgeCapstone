using Mediator;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Revenues.Command.CreateUrlPaymentRevenue;

public class CreateUrlPaymentRevenueCommand : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
    public ERevenueType Type { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}