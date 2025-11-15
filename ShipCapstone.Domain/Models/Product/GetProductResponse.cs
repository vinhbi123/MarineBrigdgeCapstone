namespace ShipCapstone.Domain.Models.Products;

public class ProductVariantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class ProductImageResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; }
}

public class GetProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Guid SupplierId { get; set; }
    public decimal? Price { get; set; }
    public bool IsHasVariant { get; set; }
    public IList<ProductVariantResponse>? Variants { get; set; }
    public IList<ProductImageResponse>? Images { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
