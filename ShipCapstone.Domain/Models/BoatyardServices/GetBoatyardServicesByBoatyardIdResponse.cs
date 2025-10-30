namespace ShipCapstone.Domain.Models.BoatyardServices;

public record GetBoatyardServicesByBoatyardIdResponse
{
    public Guid Id { get; set; }
    public string TypeService { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}