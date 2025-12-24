using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ProductVariantOptions.Command.RemoveProductVariantOption;

public class RemoveProductVariantOptionCommand : IRequest<ApiResponse>
{
    public Guid ProductVariantOptionId { get; set; }
}