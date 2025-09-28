using ShipCapstone.Domain.Entities.Common;
using ShipCapstone.Domain.Enums;

namespace ShipCapstone.Domain.Entities;

public class ShipPortHistory : EntityAuditBase<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EShipPortHistoryStatus Status { get; set; }
    public Guid PortId { get; set; }
    public Guid ShipId { get; set; }
    
    public virtual Port Port { get; set; }
    public virtual Ship Ship { get; set; }
}