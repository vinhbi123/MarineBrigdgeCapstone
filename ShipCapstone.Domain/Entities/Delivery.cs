using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class Delivery : EntityBase<Guid>
{
    public DateTime DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public EDeliveryStatus Status { get; set; }
    public Guid ShipId { get; set; }
    public Guid OrderId { get; set; }
    
    public virtual Ship Ship { get; set; }
    public virtual Order Order { get; set; }
}