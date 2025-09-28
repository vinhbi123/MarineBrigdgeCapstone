using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class OrderItem : EntityBase<Guid>
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? ProductOptionName { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductVariantId { get; set; }
    
    public virtual Order Order { get; set; }
    public virtual ProductVariant ProductVariant { get; set; }
}