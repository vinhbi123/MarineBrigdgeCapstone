using ShipCapstone.Domain.Entities.Common;

namespace ShipCapstone.Domain.Entities;

public class Ship : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? RegisterNo { get; set; }
    public int? BuildYear { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public Guid AccountId { get; set; }
    
    public virtual Account Account { get; set; }
    
    public virtual ICollection<ShipPortHistory>? ShipPortHistories { get; set; } = new List<ShipPortHistory>();
    public virtual ICollection<Delivery>? Deliveries { get; set; } = new List<Delivery>();
}