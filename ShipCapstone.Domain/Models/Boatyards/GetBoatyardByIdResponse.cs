namespace ShipCapstone.Domain.Models.Boatyards;

public record GetBoatyardByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public Guid AccountId { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}