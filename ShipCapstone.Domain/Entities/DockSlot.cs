using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class DockSlot : EntityBase<Guid>
{
    public string Name { get; set; }
    public DateTime AssignedFrom { get; set; }
    public DateTime? AssignedUntil { get; set; }
    public bool IsActive { get; set; }
    public Guid BoatyardId { get; set; }
    
    public virtual Boatyard Boatyard { get; set; }
    public virtual ICollection<Booking>? Bookings { get; set; } = new List<Booking>();
}