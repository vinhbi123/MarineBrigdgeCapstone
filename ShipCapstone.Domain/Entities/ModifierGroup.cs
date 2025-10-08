using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class ModifierGroup : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public Guid SupplierId { get; set; }
    
    public virtual Supplier Supplier { get; set; }
    public virtual ICollection<ModifierOption> ModifierOptions { get; set; }
    public virtual ICollection<ProductModifierGroup>? ProductModifierGroups { get; set; }
}