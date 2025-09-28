using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class ModifierOption : EntityBase<Guid>
{
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public Guid ModifierGroupId { get; set; }
    
    public virtual ModifierGroup ModifierGroup { get; set; }
    public virtual ICollection<Inventory>? Inventories { get; set; } = new List<Inventory>();
}