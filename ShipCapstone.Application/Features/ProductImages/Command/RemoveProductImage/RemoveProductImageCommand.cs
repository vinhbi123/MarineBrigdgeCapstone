using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.ProductImages.Command.RemoveProductImage;

public class RemoveProductImageCommand : IRequest<ApiResponse>
{
    public Guid ProductImageId { get; set; }
}