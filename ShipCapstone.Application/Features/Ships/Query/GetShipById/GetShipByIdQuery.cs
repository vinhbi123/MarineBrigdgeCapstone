using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Ships.Query.GetShipById;

public class GetShipByIdQuery : IRequest<ApiResponse>
{
    public Guid ShipId { get; set; }
}