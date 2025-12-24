using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class ProductVariant : EntityBase<Guid>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Guid ProductId { get; set; }
    public bool IsActive { get; set; }
    public virtual Product Product { get; set; }
    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<BookingReplacementProduct>? BookingReplacementProducts { get; set; } = new List<BookingReplacementProduct>();
    public virtual ICollection<ProductVariantOption>? ProductVariantOptions { get; set; } = new List<ProductVariantOption>();
}