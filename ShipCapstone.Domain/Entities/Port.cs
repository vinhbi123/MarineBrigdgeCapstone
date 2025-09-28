using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Port : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string? City { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    
    public virtual ICollection<ShipPortHistory>? ShipPortHistories { get; set; } = new List<ShipPortHistory>();
}