namespace ShipCapstone.Domain.Models.Products;

public record GetProductByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public bool IsHasVariant { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }
    public List<ProductImageResponse> ProductImages { get; set; }
    public List<ProductVariantResponseForGetProductById> ProductVariants { get; set; }
}
public record ProductImageResponse
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
    public int? SortOrder { get; set; }
}
public record ProductVariantResponseForGetProductById
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}