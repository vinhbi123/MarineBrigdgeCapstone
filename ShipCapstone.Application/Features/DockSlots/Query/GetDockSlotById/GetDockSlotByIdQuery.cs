using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.DockSlots.Query.GetDockSlotById;

public class GetDockSlotByIdQuery : IRequest<ApiResponse>
{
    public Guid DockSlotId { get; set; }
}