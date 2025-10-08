namespace ShipCapstone.Domain.Models.ModifierOptions;

public record GetModifierOptionByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int DisplayOrder { get; set; }
}