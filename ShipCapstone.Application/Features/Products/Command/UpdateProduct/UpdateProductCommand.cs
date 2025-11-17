using Mediator;
using ShipCapstone.Domain.Models.Common;
using System.Collections.Generic;

namespace ShipCapstone.Application.Features.Products.Command.UpdateProduct;

public class UpdateProductVariantRequest
{
    public Guid? Id { get; set; } 
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class UpdateProductImageRequest
{
    public Guid? Id { get; set; } 
    public string Url { get; set; }
}

public class UpdateProductCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; } 
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public decimal? Price { get; set; } 
    public bool IsHasVariant { get; set; } = false;

    public IList<UpdateProductVariantRequest>? Variants { get; set; } = new List<UpdateProductVariantRequest>();

    public IList<UpdateProductImageRequest>? Images { get; set; } = new List<UpdateProductImageRequest>();
}
