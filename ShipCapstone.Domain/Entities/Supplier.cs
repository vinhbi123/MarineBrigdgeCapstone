using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Supplier : EntityBase<Guid>
{
    public string Name { get; set; }
    public ESupplierType Type { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
}