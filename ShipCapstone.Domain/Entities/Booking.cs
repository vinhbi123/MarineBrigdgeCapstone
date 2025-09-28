using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Booking : EntityAuditBase<Guid>
{
    public EBookingStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public EBookingType Type { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid AccountId { get; set; }
    public Guid DockSlotId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual DockSlot DockSlot { get; set; }
    public virtual ICollection<Complaint>? Complaints { get; set; } = new List<Complaint>();
    public virtual ICollection<BoatyardReview>? BoatyardReviews { get; set; } = new List<BoatyardReview>();
    public virtual ICollection<BookingService>? BookingServices { get; set; } = new List<BookingService>();
}