using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Supplier : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual ICollection<Category>? Categories { get; set; } = new List<Category>();
    public virtual ICollection<ModifierGroup>? ModifierGroups { get; set; } = new List<ModifierGroup>();
}