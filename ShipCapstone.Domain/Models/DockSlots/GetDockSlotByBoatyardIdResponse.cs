namespace ShipCapstone.Domain.Models.DockSlots;

public record GetDockSlotByBoatyardIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime AssignedFrom { get; set; }
    public DateTime? AssignedUntil { get; set; }
}