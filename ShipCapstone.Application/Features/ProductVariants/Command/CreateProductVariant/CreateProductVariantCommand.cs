using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ProductVariants.Command.CreateProductVariant;

public class CreateProductVariantCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<Guid>? ModifierOptionIds { get; set; }
}

public class CreateProductVariantRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public List<Guid>? ModifierOptionIds { get; set; }
}