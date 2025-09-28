using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class ProductVariant : EntityBase<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }
    
    public virtual Product Product { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Inventory>? Inventories { get; set; } = new List<Inventory>();
    public virtual ICollection<BookingReplacementProduct>? BookingReplacementProducts { get; set; } = new List<BookingReplacementProduct>();
}