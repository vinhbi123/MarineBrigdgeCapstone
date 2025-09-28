using ShipCapstone.Domain.Entities.Common.Interface;

namespace ShipCapstone.Domain.Entities.Common;

public class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}