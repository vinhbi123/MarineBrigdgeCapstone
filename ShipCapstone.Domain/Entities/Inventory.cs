using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Inventory : EntityAuditBase<Guid>
{
    public Guid ProductVariantId { get; set; }
    public Guid? ModifierOptionId { get; set; }
    public int Quantity { get; set; }
    
    public virtual ProductVariant ProductVariant { get; set; }
    public virtual ModifierOption? ModifierOption { get; set; }
}