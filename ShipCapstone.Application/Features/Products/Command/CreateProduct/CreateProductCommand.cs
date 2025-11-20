public Guid CategoryId { get; set; }
public decimal? Price { get; set; }
public bool IsHasVariant { get; set; }
public List<CreateProductVariantForCreateProductRequest>? ProductVariants { get; set; }
public List<CreateProductImageForCreateProductRequest> ProductImages { get; set; }
}
public class CreateProductVariantForCreateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
public class CreateProductImageForCreateProductRequest
{
    public IFormFile Image { get; set; }
    public int? SortOrder { get; set; }
}