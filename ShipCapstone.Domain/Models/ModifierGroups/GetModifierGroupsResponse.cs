namespace ShipCapstone.Domain.Models.ModifierGroups;

public record GetModifierGroupsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}