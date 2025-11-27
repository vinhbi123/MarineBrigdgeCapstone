using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Bookings.Query.GetBookingById;

public class GetBookingByIdQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}