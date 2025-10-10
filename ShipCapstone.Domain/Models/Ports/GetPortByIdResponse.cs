namespace ShipCapstone.Domain.Models.Ports;

public record GetPortByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string? City { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}