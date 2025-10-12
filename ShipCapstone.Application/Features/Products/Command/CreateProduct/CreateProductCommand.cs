using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public decimal? Price { get; set; }
    public bool IsHasVariant { get; set; }
}
public class CreateProductVariantForCreateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
