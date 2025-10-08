using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Boatyard : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual ICollection<DockSlot> DockSlots { get; set; } = new List<DockSlot>();
    public virtual ICollection<BoatyardService>? BoatyardServices { get; set; } = new List<BoatyardService>();
}