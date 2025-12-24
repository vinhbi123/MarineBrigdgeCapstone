using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ProductVariantOptions.Command.AddProductVariantOption;

public class AddProductVariantOptionCommand : IRequest<ApiResponse>
{
    public Guid ProductVariantId { get; set; }
    public Guid ModifierOptionId { get; set; }
}