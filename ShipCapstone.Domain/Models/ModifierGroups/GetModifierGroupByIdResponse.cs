namespace ShipCapstone.Domain.Models.ModifierGroups;

public record GetModifierGroupByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; } 
    public List<GetModiferOptionForModifierGroupResponse> ModifierOptions { get; set; } = new();
}
public record GetModiferOptionForModifierGroupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int DisplayOrder { get; set; }
}