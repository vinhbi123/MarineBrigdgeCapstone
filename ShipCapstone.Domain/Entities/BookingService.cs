using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class BookingService : EntityBase<Guid>
{
    public Guid BookingId { get; set; }
    public Guid BoatyardServiceId { get; set; }
    public EBookingServiceStatus Status { get; set; }
    
    public virtual Booking Booking { get; set; }
    public virtual BoatyardService BoatyardService { get; set; }
    public virtual ICollection<BookingReplacementProduct>? BookingReplacementProducts { get; set; } = new List<BookingReplacementProduct>();
}