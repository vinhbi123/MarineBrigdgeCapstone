using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class BookingReplacementProduct : EntityAuditBase<Guid>
{
    public string? Note { get; set; }
    public int Quantity { get; set; }
    public Guid BookingServiceId { get; set; }
    public Guid ProductVariantId { get; set; }
    
    public virtual BookingService BookingService { get; set; }
    public virtual ProductVariant ProductVariant { get; set; }
}