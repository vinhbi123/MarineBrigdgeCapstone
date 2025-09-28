using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class ProductImage : EntityBase<Guid>
{
    public string ImageUrl { get; set; }
    public int? SortOrder { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    
}