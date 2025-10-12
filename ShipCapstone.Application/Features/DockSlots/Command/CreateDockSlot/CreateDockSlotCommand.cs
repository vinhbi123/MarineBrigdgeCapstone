using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.DockSlots.Command.CreateDockSlot;

public class CreateDockSlotCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public DateTime AssignedFrom { get; set; }
    public DateTime? AssignedUntil { get; set; }
}