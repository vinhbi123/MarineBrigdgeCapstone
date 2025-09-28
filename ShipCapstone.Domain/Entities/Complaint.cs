using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Complaint : EntityAuditBase<Guid>
{
    public string Content { get; set; }
    public EComplaintStatus Status { get; set; }
    public Guid AccountId { get; set; }
    public Guid? OrderId { get; set; }
    public Guid? BookingId { get; set; }
    
    public virtual Account Account { get; set; }
    public virtual Order? Order { get; set; }
    public virtual Booking? Booking { get; set; }
}