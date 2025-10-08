namespace ShipCapstone.Domain.Models.Ships;

public record GetShipsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? ImoNumber { get; set; }
    public string? RegisterNo { get; set; }
    public int? BuildYear { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}