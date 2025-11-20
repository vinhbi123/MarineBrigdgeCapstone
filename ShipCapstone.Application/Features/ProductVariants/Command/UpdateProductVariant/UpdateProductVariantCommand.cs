using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ProductVariants.Command.UpdateProductVariant;

public class UpdateProductVariantCommand : IRequest<ApiResponse>
{
    public Guid ProductVariantId { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}
public class UpdateProductVariantRequest
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}