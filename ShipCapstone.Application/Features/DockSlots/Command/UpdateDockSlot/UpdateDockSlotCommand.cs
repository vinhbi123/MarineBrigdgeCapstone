using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.DockSlots.Command.UpdateDockSlot;

public class UpdateDockSlotCommand : IRequest<ApiResponse>
{
    public Guid DockSlotId { get; set; }
    public string Name { get; set; }
    public DateTime AssignedFrom { get; set; }
    public DateTime? AssignedUntil { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateDockSlotRequest
{
    public string Name { get; set; }
    public DateTime AssignedFrom { get; set; }
    public DateTime? AssignedUntil { get; set; }
    public bool IsActive { get; set; }
}