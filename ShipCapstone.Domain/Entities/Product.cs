using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Product : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsHasVariant { get; set; }
    public Guid CategoryId { get; set; }
    public Guid SupplierId { get; set; }
    public bool IsActive { get; set; }
    public virtual Category Category { get; set; }
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<Review>? Reviews { get; set; } = new List<Review>();
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}