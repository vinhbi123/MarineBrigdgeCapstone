using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class ProductModifierGroup : EntityBase<Guid>
{
    public Guid ProductId { get; set; }
    public Guid ModifierGroupId { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual ModifierGroup ModifierGroup { get; set; }
}