using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class BoatyardReview : EntityAuditBase<Guid>
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public Guid BookingId { get; set; }
    
    public virtual Booking Booking { get; set; }
}