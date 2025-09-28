using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class BoatyardService : EntityAuditBase<Guid>
{
    public string TypeService { get; set; }
    public bool IsActive { get; set; }
    public decimal Price { get; set; }
    public Guid BoatyardId { get; set; }
    
    public virtual Boatyard Boatyard { get; set; }
    public virtual ICollection<BookingService>? BookingServices { get; set; } = new List<BookingService>();
}