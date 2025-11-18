using Mediator;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Features.Products.Command.DeleteProduct;

public class DeleteProductCommand : IRequest<ApiResponse>
{
    public Guid ProductId { get; set; }
}
