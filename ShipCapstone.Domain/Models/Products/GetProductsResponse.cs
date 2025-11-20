namespace ShipCapstone.Domain.Models.Products;

public record GetProductsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public bool IsHasVariant { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
